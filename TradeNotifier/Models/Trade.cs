using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{
    public class Trade
    {
        public enum SideOptions
        {
            None,
            Buy,
            Sell
        }

        public enum StatusOptions
        {
            None,
            Canceled,
            Filled,
            New
        }


        public enum TypeOptions
        {
            None,
            Limit,
            Market,
            Stop
        }

        [Required]
        public DateTime TimeStamp { get; set; }


        public decimal FillPrice { get; set; }

        [Required]
        public decimal OrderValue { get; set; }


        public decimal Price { get; set; }


        public decimal StopPrice { get; set; }

        [Required]
        public int Filled { get; set; }

        [Required]
        public int Remaining { get; set; }

        [Required]
        public StatusOptions Status { get; set; }

        [Key]
        public string Id { get; set; }

        [Required]
        public string Quantity { get; set; }

        [Required]
        public SideOptions Side { get; set; }

        [Required]
        public string Symbol { get; set; }


        public string Text { get; set; }

        [Required]
        public TypeOptions Type { get; set; }
    }
}
