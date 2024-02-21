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
    public abstract class ModelEditBase<T> : ViewModelBase where T : DbModelBase, new()
    {
        #region Properties

        private T _editedModel = null;
        public T EditedModel
        {
            get { return _editedModel; }
            set { this.RaiseAndSetIfChanged(ref _editedModel, value); }
        }

        private T _formModel = null;
        public T FormModel
        {
            get { return _formModel; }
            set { this.RaiseAndSetIfChanged(ref _formModel, value); }
        }

        private string _errorText;
        public string ErrorText
        {
            get { return _errorText; }
            set { this.RaiseAndSetIfChanged(ref _errorText, value); }
        }



        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> AddModelCommand { get; }
        public ReactiveCommand<Unit, Unit> EditModelCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteModelCommand { get; }

        #endregion

        public ModelEditBase()
        {
            IObservable<bool> editModelCanExecute = this.WhenAnyValue(
                x => x.EditedModel)
                .Select((selectedModel) => selectedModel != null);

            AddModelCommand = ReactiveCommand.Create(AddModel);
            EditModelCommand = ReactiveCommand.Create(EditModel, editModelCanExecute);
            DeleteModelCommand = ReactiveCommand.Create(DeleteModel, editModelCanExecute);

            LoadSources();
        }
        protected abstract void LoadSources();
        public void SetEditedModel(T model)
        {
            if (model != null)
            {
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

            if (!SQLAccess.InsertSingle(newModel))
                return;
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
        private void DeleteModel()
        {

        }
        protected abstract string AddModelCheck();
        protected abstract string EditModelCheck();
        protected abstract string DeleteModelCheck();
        protected abstract void ClearForm();
    }
}
