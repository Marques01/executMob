namespace UI.Components.Pages.Breakdown
{
    public partial class Index
    {
        private IAsyncEnumerable<Domain.Entities.Breakdown>? _breakdowns;

        private List<Domain.Entities.Breakdown> _breakdownsList = new();

        protected override async Task OnInitializedAsync()
        {
            await GetBreakdownsAsync();
        }

        private async Task GetBreakdownsAsync()
        {
            _breakdowns = _breakdownServices.GetBreakdownsAsync();

            await foreach (var breakdown in _breakdowns)
            {
                _breakdownsList.Add(breakdown);
            }
        }
    }
}