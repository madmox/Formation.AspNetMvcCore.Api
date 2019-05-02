using Newtonsoft.Json;
using System;
using TodoApi.Entities;

namespace TodoApi.DTO.Commands
{
    /// <summary>
    /// Contient les informations nécessaires pour créer un nouveau TODO item
    /// </summary>
    public class TodoItemCreateCommand
    {
        /// <summary>
        /// Le libellé du TODO item.
        /// Ne doit pas être renseigné si 'copy_from' est renseigné aussi. Obligatoire sinon.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// L'identifiant d'un autre TODO item à partir duquel le libellé sera copié.
        /// Ne doit pas être renseigné si 'label' est renseigné.
        /// </summary>
        [JsonProperty("copy_from")]
        public Guid? CopyFrom { get; set; }

        public TodoItemCreateCommand()
        {
        }

        public void MergeIntoEntity(TodoItem item, TodoItem sourceItem)
        {
            if (sourceItem != null)
            {
                item.Label = sourceItem.Label;
            }
            else
            {
                item.Label = this.Label;
            }
        }
    }
}
