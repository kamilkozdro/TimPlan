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
    public abstract class ModelEditViewModel<T> : ViewModelBase where T : DbModelBase<T>, new()
    {
        #region Properties

        private string _searchItemText = "";
        public string SearchItemText
        {
            get { return _searchItemText; }
            set { this.RaiseAndSetIfChanged(ref _searchItemText, value); }
        }
        private ObservableCollection<T> _Items = new ObservableCollection<T>();
        public ObservableCollection<T> Items
        {
            get { return _Items; }
            set { this.RaiseAndSetIfChanged(ref _Items, value); }
        }
        private T _selectedItem = null;
        public T SelectedItem
        {
            get { return _selectedItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        }

        protected ReadOnlyCollection<T> _loadedItems;

        protected IObservable<bool> addItemCheck;
        protected IObservable<bool> editItemCheck;
        protected IObservable<bool> deleteItemCheck;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> AddItemCommand { get; }
        public ReactiveCommand<Unit, Unit> EditItemCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteItemCommand { get; }

        #endregion

        public ModelEditViewModel()
        {
            this.WhenAnyValue(o => o.SearchItemText)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Subscribe(FilterItemList);

            this.WhenAnyValue(o => o.SelectedItem)
                .Subscribe(OnItemSelection);

            SetupCommandsCanExecute();

            AddItemCommand = ReactiveCommand.Create(AddItem, addItemCheck);
            EditItemCommand = ReactiveCommand.Create(EditItem, editItemCheck);
            DeleteItemCommand = ReactiveCommand.Create(DeleteItem, deleteItemCheck);

            UpdateItemsSource();
            ClearForm();
        }

        protected abstract void SetupCommandsCanExecute();
        protected abstract T GetNewItemFromForm();
        private void UpdateItemsSource()
        {
            _loadedItems = new ReadOnlyCollection<T>(
                SQLAccess.SelectAll<T>());

            FilterItemList(SearchItemText);
        }
        protected abstract void OnItemSelection(T selectedItem);
        protected virtual void AddItem()
        {
            if (!SQLAccess.InsertSingle<T>(GetNewItemFromForm()))
            {
                //TODO: could not add new item message
                Debug.WriteLine("Could not add new item");

            }

            //TODO: new item added successfully message
            Debug.WriteLine("New item added successfully");
            UpdateItemsSource();
            ClearForm();
        }
        protected virtual void EditItem()
        {
            if (!SQLAccess.UpdateSingle<T>(GetNewItemFromForm()))
            {
                //TODO: could not add new item message
                Debug.WriteLine("Could not edit item");

            }

            //TODO: new item added successfully message
            Debug.WriteLine("Item edited successfully");
            UpdateItemsSource();
            ClearForm();
        }
        protected virtual async void DeleteItem()
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            bool dialogResult = await windowService.ShowDialogYesNo("Czy na pewno chcesz usunąć rekord?");
            if (!dialogResult)
                return;

            if (!SQLAccess.DeleteSingle<T>(SelectedItem.Id))
            {
                //TODO: could not delete machine message
                Debug.WriteLine("Could not delete item");
                return;
            }
            //TODO: machine deleted successfully message
            Debug.WriteLine("Item deleted successfully");
            UpdateItemsSource();
            ClearForm();

        }
        protected abstract bool AddItemCheck();
        protected abstract bool EditItemCheck();
        protected abstract bool DeleteItemCheck();
        protected abstract void ClearForm();
        protected abstract void FilterItemList(string filterText);
    }
}
