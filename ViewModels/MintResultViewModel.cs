// ViewModels/MintResultViewModel.cs
using System;

namespace mu_marketplaceV0.ViewModels
{
    /// <summary>
    /// Holds the results of a successful mint so the view can display
    /// a polished confirmation page with explorer links.
    /// </summary>
    public class MintResultViewModel
    {
        public string? MintAddress { get; set; }
        public string? TransactionSignature { get; set; }
        public string? ExplorerUrl { get; set; }

        // Raw logs (stdout / stderr) are helpful for diagnostics
        public string RawOutput { get; set; } = string.Empty;
        public string RawError  { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
    }
}
