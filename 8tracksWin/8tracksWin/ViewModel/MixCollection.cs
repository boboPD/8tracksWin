using Windows.UI.Xaml.Data;
using System.Collections.ObjectModel;
using Common.Model;
using Common.Search;
using System.Threading.Tasks;
using Windows.Foundation;
using System;

namespace _8tracksWin.ViewModel
{
    public class MixCollection : ObservableCollection<Mix>, ISupportIncrementalLoading
    {
        public SearchResult Collection { get; set; }
        bool isFirstLoad;

        public MixCollection(SearchResult r)
        {
            Collection = r;
            foreach (var item in Collection.ResultSet.mixes)
                this.Add(item);
            isFirstLoad = true;
        }

        public bool HasMoreItems
        {
            get
            {
                    return !(Collection.ResultSet.pagination.total_pages == Collection.ResultSet.pagination.current_page);
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run<LoadMoreItemsResult>(async () =>
             {
                 if (isFirstLoad)
                     isFirstLoad = false;
                 else
                     await Collection.MoveToNextPage();

                 Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                       () =>
                       {
                           foreach (var item in Collection.ResultSet.mixes)
                           {
                               this.Add(item);
                           }
                       });

                 return new LoadMoreItemsResult() { Count = (uint)Collection.ResultSet.pagination.per_page };
             }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }
}
