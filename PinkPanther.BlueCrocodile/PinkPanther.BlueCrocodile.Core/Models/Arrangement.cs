namespace PinkPanther.BlueCrocodile.Core.Models
{
    /// <summary>
    ///     There are multiple arrangements available. Each arrangement is by default available in all cinemas and for all
    ///     movies.
    /// </summary>
    public class Arrangement
    {
        public string Name { get; set; }
        public decimal Ammount { get; set; } = 0;
        public string Description { get; set; }
    }
}