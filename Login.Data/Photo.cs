
using System;
using System.ComponentModel.DataAnnotations;
namespace Login.Data
{

    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public String Photo64 { get; set; }
    }
}