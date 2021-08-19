/*
 * Inside.TestTask.MC1
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = Org.OpenAPITools.Client.OpenAPIDateConverter;

namespace Org.OpenAPITools.Model
{
    /// <summary>
    /// Message
    /// </summary>
    [DataContract(Name = "Message")]
    public partial class Message : IEquatable<Message>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="sessionId">sessionId.</param>
        /// <param name="mC1Timestamp">mC1Timestamp.</param>
        /// <param name="mC2Timestamp">mC2Timestamp.</param>
        /// <param name="mC3Timestamp">mC3Timestamp.</param>
        /// <param name="endTimestamp">endTimestamp.</param>
        public Message(int id = default(int), int sessionId = default(int), DateTime mC1Timestamp = default(DateTime), DateTime mC2Timestamp = default(DateTime), DateTime mC3Timestamp = default(DateTime), DateTime endTimestamp = default(DateTime))
        {
            this.Id = id;
            this.SessionId = sessionId;
            this.MC1Timestamp = mC1Timestamp;
            this.MC2Timestamp = mC2Timestamp;
            this.MC3Timestamp = mC3Timestamp;
            this.EndTimestamp = endTimestamp;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets SessionId
        /// </summary>
        [DataMember(Name = "session_id", EmitDefaultValue = false)]
        public int SessionId { get; set; }

        /// <summary>
        /// Gets or Sets MC1Timestamp
        /// </summary>
        [DataMember(Name = "MC1_timestamp", EmitDefaultValue = false)]
        public DateTime MC1Timestamp { get; set; }

        /// <summary>
        /// Gets or Sets MC2Timestamp
        /// </summary>
        [DataMember(Name = "MC2_timestamp", EmitDefaultValue = false)]
        public DateTime MC2Timestamp { get; set; }

        /// <summary>
        /// Gets or Sets MC3Timestamp
        /// </summary>
        [DataMember(Name = "MC3_timestamp", EmitDefaultValue = false)]
        public DateTime MC3Timestamp { get; set; }

        /// <summary>
        /// Gets or Sets EndTimestamp
        /// </summary>
        [DataMember(Name = "end_timestamp", EmitDefaultValue = false)]
        public DateTime EndTimestamp { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Message {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  SessionId: ").Append(SessionId).Append("\n");
            sb.Append("  MC1Timestamp: ").Append(MC1Timestamp).Append("\n");
            sb.Append("  MC2Timestamp: ").Append(MC2Timestamp).Append("\n");
            sb.Append("  MC3Timestamp: ").Append(MC3Timestamp).Append("\n");
            sb.Append("  EndTimestamp: ").Append(EndTimestamp).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as Message);
        }

        /// <summary>
        /// Returns true if Message instances are equal
        /// </summary>
        /// <param name="input">Instance of Message to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Message input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Id == input.Id ||
                    this.Id.Equals(input.Id)
                ) && 
                (
                    this.SessionId == input.SessionId ||
                    this.SessionId.Equals(input.SessionId)
                ) && 
                (
                    this.MC1Timestamp == input.MC1Timestamp ||
                    (this.MC1Timestamp != null &&
                    this.MC1Timestamp.Equals(input.MC1Timestamp))
                ) && 
                (
                    this.MC2Timestamp == input.MC2Timestamp ||
                    (this.MC2Timestamp != null &&
                    this.MC2Timestamp.Equals(input.MC2Timestamp))
                ) && 
                (
                    this.MC3Timestamp == input.MC3Timestamp ||
                    (this.MC3Timestamp != null &&
                    this.MC3Timestamp.Equals(input.MC3Timestamp))
                ) && 
                (
                    this.EndTimestamp == input.EndTimestamp ||
                    (this.EndTimestamp != null &&
                    this.EndTimestamp.Equals(input.EndTimestamp))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                hashCode = hashCode * 59 + this.Id.GetHashCode();
                hashCode = hashCode * 59 + this.SessionId.GetHashCode();
                if (this.MC1Timestamp != null)
                    hashCode = hashCode * 59 + this.MC1Timestamp.GetHashCode();
                if (this.MC2Timestamp != null)
                    hashCode = hashCode * 59 + this.MC2Timestamp.GetHashCode();
                if (this.MC3Timestamp != null)
                    hashCode = hashCode * 59 + this.MC3Timestamp.GetHashCode();
                if (this.EndTimestamp != null)
                    hashCode = hashCode * 59 + this.EndTimestamp.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
