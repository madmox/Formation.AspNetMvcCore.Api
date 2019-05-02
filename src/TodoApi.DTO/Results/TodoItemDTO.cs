using Newtonsoft.Json;
using System;
using TodoApi.Entities;

namespace TodoApi.DTO.Results
{
    /// <summary>
    /// Représente l'ensemble des informations relatives à un TODO item et accessibles par le client.
    /// </summary>
    public class TodoItemDTO
    {
        /// <summary>
        /// L'identifiant du TODO item
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Le libellé du TODO item
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Le date à laquelle le TODO item a été créé
        /// </summary>
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Le date à laquelle le TODO item a été mis à jour pour la dernière fois
        /// </summary>
        [JsonProperty("updated")]
        public DateTime Updated { get; set; }

        public TodoItemDTO()
        {
        }

        public TodoItemDTO(TodoItem item)
        {
            this.Id = item.Id;
            this.Label = item.Label;
            this.Created = item.Created;
            this.Updated = item.Updated;
        }
    }
}
