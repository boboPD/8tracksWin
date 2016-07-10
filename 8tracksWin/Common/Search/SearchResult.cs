using Common.Model;

namespace Common.Search
{
    public class SearchResult
    {
        public MixSet ResultSet { get; private set; }

        public SearchResult(MixSet results)
        {
            ResultSet = results;
        }

        private async System.Threading.Tasks.Task<bool> MovePage(bool moveForward)
        {
            int? page = moveForward ? ResultSet.pagination.next_page : ResultSet.pagination.previous_page;
            if (page.HasValue)
            {
                ResultSet = (await Search.MixSearch.GetPageForSet(ResultSet.path, page.Value)).ResultSet;
                return true;
            }
            else
                return false;
        }
        
        public async System.Threading.Tasks.Task<bool> MoveToNextPage()
        {
            return (await MovePage(true));
        }

        public async System.Threading.Tasks.Task<bool> MoveToPrevPage()
        {
            return (await MovePage(false));
        }
    }
}
