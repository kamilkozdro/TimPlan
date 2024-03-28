using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using TimPlan.Lib;
using TimPlan.Models;
using TimPlan.Services;

namespace TimPlan.ViewModels
{

    public enum EditWindowType { Add, Edit, View };
    public abstract class ModelEditBase<T> : ViewModelBase where T : DbModelBase, new()
    {
        #region Properties
                
        private T _editedModel = null;
        public T EditedModel
        {
            get { return _editedModel; }
            set { this.RaiseAndSetIfChanged(ref _editedModel, value); }
        }

        private T _formModel = new T();
        public T FormModel
        {
            get { return _formModel; }
            set { this.RaiseAndSetIfChanged(ref _formModel, value); }
        }

        private string _errorText = string.Empty;
        public string ErrorText
        {
            get { return _errorText; }
            set { this.RaiseAndSetIfChanged(ref _errorText, value); }
        }

        #endregion

        #region Access Properties

        private bool _canEditForm = false;
        public bool CanEditForm
        {
            get { return _canEditForm; }
            set { this.RaiseAndSetIfChanged(ref _canEditForm, value); }
        }

        private bool _canAddModel = false;
        public bool CanAddModel
        {
            get { return _canAddModel; }
            set { this.RaiseAndSetIfChanged(ref _canAddModel, value); }
        }

        private bool _canEditModel = false;
        public bool CanEditModel
        {
            get { return _canEditModel; }
            set { this.RaiseAndSetIfChanged(ref _canEditModel, value); }
        }

        private bool _canDeleteModel = false;
        public bool CanDeleteModel
        {
            get { return _canDeleteModel; }
            set { this.RaiseAndSetIfChanged(ref _canDeleteModel, value); }
        }

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> AddModelCommand { get; }
        public ReactiveCommand<Unit, Unit> EditModelCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteModelCommand { get; }
        public ReactiveCommand<T, T> ReturnResultCommand { get; }

        #endregion

        public ModelEditBase()
        {
            IObservable<bool> editModelCanExecute = this.WhenAnyValue(
                x => x.EditedModel)
                .Select((selectedModel) => selectedModel != null);

            AddModelCommand = ReactiveCommand.Create(AddModel);
            EditModelCommand = ReactiveCommand.Create(EditModel, editModelCanExecute);
            DeleteModelCommand = ReactiveCommand.Create(DeleteModel, editModelCanExecute);
            ReturnResultCommand = ReactiveCommand.Create<T, T>(model =>
            {
                return model;
            });

            LoadSources();
            SetEditWindowType(EditWindowType.View);
        }
        public void SetEditWindowType(EditWindowType accessType)
        {
            switch (accessType)
            {
                case EditWindowType.Add:
                    {
                        CanEditForm = true;
                        CanAddModel = true;
                        CanEditModel = false;
                        CanDeleteModel = false;
                        break;
                    }
                case EditWindowType.Edit:
                    {
                        CanEditForm = true;
                        CanAddModel = false;
                        CanEditModel = true;
                        CanDeleteModel = true;
                        break;
                    }
                case EditWindowType.View:
                    {
                        CanEditForm = false;
                        CanAddModel = false;
                        CanEditModel = false;
                        CanDeleteModel = false;
                        break;
                    }
            }
        }
        protected abstract void LoadSources();
        public void SetEditedModel(T model)
        {
            if (model != null)
            {
                EditedModel = model;
                FormModel = AnnotationHelper.DeepCopyReflection(model);
                SetFormFromModel(model);
            }
            else
            {
                ClearForm();
            }
        }
        protected virtual T GetModelFromForm()
        {
            return FormModel;
        }
        protected abstract void SetFormFromModel(T model);
        protected virtual bool AddModelAdditionalAction()
        {
            return true;
        }
        private void AddModel()
        {
            ErrorText = AddModelCheck();
            if (!string.IsNullOrEmpty(ErrorText))
                return;

            if (!AddModelAdditionalAction())
                return;

            T newModel = GetModelFromForm();

            int newModelId = SQLAccess.InsertSingle(newModel);

            if (newModelId < 1)
                return;

            newModel.Id = newModelId;

            ReturnResultCommand.Execute(newModel).Subscribe();
        }
        protected virtual bool EditModelAdditionalAction()
        { 
            return true;
        }
        private void EditModel()
        {
            ErrorText = EditModelCheck();
            if (!string.IsNullOrEmpty(ErrorText))
                return;

            if (!EditModelAdditionalAction())
                return;

            T newModel = GetModelFromForm();
            if (!SQLAccess.UpdateSingle(newModel))
            {
                return;
            }
        }        
        protected virtual bool DeleteModelAdditionalAction()
        {
            return true;
        }
        private async void DeleteModel()
        {
            var windowService = App.Current.Services.GetService<IWindowService>();
            bool dialogResult = await windowService.ShowDialogYesNo("Are you sure?");

            if (dialogResult == false)
                return;

            if (!SQLAccess.DeleteSingle<T>(EditedModel.Id))
                return;

            ReturnResultCommand.Execute(null).Subscribe();
        }
        protected abstract string AddModelCheck();
        protected abstract string EditModelCheck();
        protected abstract string DeleteModelCheck();
        protected abstract void ClearForm();
    }
}
