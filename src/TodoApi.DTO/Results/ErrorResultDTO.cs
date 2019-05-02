using Newtonsoft.Json;

namespace TodoApi.DTO.Results
{
    /// <summary>
    /// Objet retourné en cas d'erreur métier et contenant des informations
    /// suffisantes pour en déterminer la cause.
    /// </summary>
    public class ErrorResultDTO
    {
        /// <summary>
        /// Message technique descriptif de l'erreur
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        public ErrorResultDTO()
        {
        }

        public ErrorResultDTO(string message)
        {
            this.Message = message;
        }
    }
}
