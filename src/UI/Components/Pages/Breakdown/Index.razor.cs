using Microsoft.Fast.Components.FluentUI;

namespace UI.Components.Pages.Breakdown
{
    public partial class Index
    {
        private bool
            _isLoading = false;

        private List<Domain.Entities.Breakdown> _breakdownsList = new();

        private PaginationState _pagination = new() { ItemsPerPage = 10 };

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            await GetBreakdownsAsync();

            _isLoading = false;
        }

        private async Task GetBreakdownsAsync()
        {
            try
            {
                var breakdowns = await _breakdownServices.GetBreakdownsIEnumerableAsync();

                foreach (var breakdown in breakdowns)
                {
                    _breakdownsList.Add(breakdown);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void BreakdownDetails(int breakdownId)
        {
            try
            {
                _navigationManager.NavigateTo($"/breakdownviewer/{breakdownId}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}