using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using TimPlan.Lib;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public abstract class ModelSearchEditBase<T> : ModelEditBase<T> where T : DbModelBase, new()
    {
        #region Properties

        private string _searchModelText = "";
        public string SearchModelText
        {
            get { return _searchModelText; }
            set { this.RaiseAndSetIfChanged(ref _searchModelText, value); }
        }
        private readonly ReadOnlyObservableCollection<T> _filteredModelCollection;
        public ReadOnlyObservableCollection<T> FilteredModelCollection
        {
            get { return _filteredModelCollection; }
        }
        private SourceCache<T, int> _sourceCache = new(x => x.Id);

        #endregion

        #region Commands



        #endregion

        public ModelSearchEditBase()
        {

            UpdateItemsSource();

            var filterPredicate = this.WhenAnyValue(o => o.SearchModelText)
                                      .Throttle(TimeSpan.FromMilliseconds(500))
                                      .DistinctUntilChanged()
                                      .Select(SearchModelFilter);

            _sourceCache.Connect()
                        .RefCount()
                        .Filter(filterPredicate)
                        .Sort(SearchModelSort())
                        .Bind(out _filteredModelCollection)
                        .DisposeMany()
                        .Subscribe();

            this.WhenAnyValue(o => o.EditedModel)
                .Subscribe(OnItemSelection);
        }
        protected abstract Func<T, bool> SearchModelFilter(string text);
        protected abstract IComparer<T> SearchModelSort();
        private void UpdateItemsSource()
        {
            IEnumerable<T> modelSource = SQLAccess.SelectAll<T>();

            foreach (T model in modelSource)
            {
                _sourceCache.AddOrUpdate(model);
            }
        }
        private void OnItemSelection(T selectedItem)
        {
            SetEditedModel(selectedItem);
        }
    }
}
