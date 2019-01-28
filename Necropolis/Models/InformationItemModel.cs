using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Necropolis.Models
{
    public class InformationItemModel
    {
        public string Id { get; set; }
        public InformationItemType Type { get; set; }
        public DateTime Date { get; set; }
        public bool Published { get; set; }
        public bool AddedToMenu { get; set; }
        public string RouteName { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }

    public enum InformationItemType
    {
        Информационная_статья,
        Вопрос
    }
}