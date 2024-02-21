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
    public abstract class ModelSearchEditBase<TModel> : ModelEditBase<TModel> where TModel : DbModelBase, new()
    {
        #region Properties

        private string _searchModelText = "";
        public string SearchModelText
        {
            get { return _searchModelText; }
            set { this.RaiseAndSetIfChanged(ref _searchModelText, value); }
        }
        private readonly ReadOnlyObservableCollection<TModel> _filteredModelCollection;
        public ReadOnlyObservableCollection<TModel> FilteredModelCollection
        {
            get { return _filteredModelCollection; }
        }
        private SourceCache<TModel, int> _sourceCache = new(x => x.Id);

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
        protected abstract Func<TModel, bool> SearchModelFilter(string text);
        protected abstract IComparer<TModel> SearchModelSort();
        private void UpdateItemsSource()
        {
            IEnumerable<TModel> modelSource = SQLAccess.SelectAll<TModel>();

            foreach (TModel model in modelSource)
            {
                _sourceCache.AddOrUpdate(model);
            }
        }
        private void OnItemSelection(TModel selectedItem)
        {
            SetEditedModel(selectedItem);
        }
    }
}
