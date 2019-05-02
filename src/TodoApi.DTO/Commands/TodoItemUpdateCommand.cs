using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TodoApi.Entities;

namespace TodoApi.DTO.Commands
{
    /// <summary>
    /// Contient les informations nécessaires pour mettre à jour un TODO item
    /// </summary>
    public class TodoItemUpdateCommand
    {
        /// <summary>
        /// Le libellé du TODO item.
        /// </summary>
        [JsonProperty("label")]
        [Required]
        public string Label { get; set; }

        public TodoItemUpdateCommand()
        {
        }

        public void MergeIntoEntity(TodoItem item)
        {
            item.Label = this.Label;
        }
    }
}
