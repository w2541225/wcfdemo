using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

namespace HelpPageEndpointBehavior
{
    class HelpPageMessage : Message
    {
        MessageHeaders headers;
        MessageProperties properties;
        ServiceEndpoint endpoint;
        string companyName;

        public HelpPageMessage(ServiceEndpoint endpoint, string companyName)
        {
            this.headers = new MessageHeaders(MessageVersion.None);
            this.properties = InitializeMessageProperties();
            this.endpoint = endpoint;
            this.companyName = companyName;
        }

        public override MessageHeaders Headers
        {
            get { return this.headers; }
        }

        public override MessageProperties Properties
        {
            get { return this.properties; }
        }

        public override MessageVersion Version
        {
            get { return MessageVersion.None; }
        }

        protected override void OnWriteBodyContents( XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("html");
            writer.WriteStartElement("head");
            writer.WriteElementString("title", "Better help page, presented by " + this.companyName);
            writer.WriteElementString("style", this.CreateStyleValue());
            writer.WriteEndElement();
            writer.WriteStartElement("body");
            writer.WriteElementString("h1", "Endpoint help page, by " + this.companyName);

            this.WriteEndpointDetails(writer);
            this.WriteEndpointBehaviors(writer);
            this.WriteContractOperations(writer);

            writer.WriteEndElement(); // body
            writer.WriteEndElement(); // html
        }

        private string CreateStyleValue()
        {
            return "table { border-collapse: collapse; border-spacing: 0px; font-family: Verdana;} " +
                "table th { border-right: 2px white solid; border-bottom: 1.5px white solid; font-weight: bold; background-color: #cecf9c;} " +
                "table td { border-right: 2px white solid; border-bottom: 1.5px white solid; background-color: #e5e5cc;} " +
                "h1 { background-color: #003366; border-bottom: #336699 6px solid; color: #ffffff; font-family: Tahoma; font-size: 26px; font-weight: normal;margin: 0em 0em 10px -20px; padding-bottom: 8px; padding-left: 30px;padding-top: 16px;} " +
                "h2 { background-color: #003366; border-bottom: #336699 6px solid; color: #ffffff; font-family: Tahoma; font-size: 20px; font-weight: normal;margin: 0em 0em 10px -20px; padding-bottom: 8px; padding-left: 30px;padding-top: 16px;} " +
                ".signature { font-family: Consolas; font-size: 12px; }";
        }

        private void WriteEndpointDetails(XmlDictionaryWriter writer)
        {
            writer.WriteElementString("h2", "Endpoint details");

            writer.WriteStartElement("table");

            writer.WriteStartElement("tr");
            writer.WriteElementString("th", "Element");
            writer.WriteElementString("th", "Value");
            writer.WriteEndElement();

            writer.WriteStartElement("tr");
            writer.WriteElementString("th", "Address");
            writer.WriteElementString("td", this.endpoint.Address.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("tr");
            writer.WriteElementString("th", "Binding");
            writer.WriteElementString("td", this.endpoint.Binding.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("tr");
            writer.WriteElementString("th", "Contract Type");
            writer.WriteElementString("td", this.endpoint.Contract.ContractType.FullName);
            writer.WriteEndElement();

            writer.WriteEndElement(); // </table>
        }

        private void WriteEndpointBehaviors(XmlDictionaryWriter writer)
        {
            writer.WriteElementString("h2", "Endpoint behaviors");

            writer.WriteStartElement("table");

            writer.WriteStartElement("tr");
            writer.WriteElementString("th", "Index");
            writer.WriteElementString("th", "Type");
            writer.WriteEndElement();

            for (int i = 0; i < this.endpoint.Behaviors.Count; i++)
            {
                IEndpointBehavior behavior = this.endpoint.Behaviors[i];
                writer.WriteStartElement("tr");
                writer.WriteElementString("td", i.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("td", behavior.GetType().FullName);
                writer.WriteEndElement();
            }

            writer.WriteEndElement(); // </table>
        }

        private void WriteContractOperations(XmlDictionaryWriter writer)
        {
            writer.WriteElementString("h2", "Operations");

            writer.WriteStartElement("table");

            writer.WriteStartElement("tr");
            writer.WriteElementString("th", "Name");
            writer.WriteElementString("th", "Signature");
            writer.WriteElementString("th", "Description");
            writer.WriteEndElement();

            foreach (OperationDescription operation in this.endpoint.Contract.Operations)
            {
                writer.WriteStartElement("tr");
                writer.WriteElementString("td", operation.Name);
                writer.WriteStartElement("td");
                writer.WriteAttributeString("class", "signature");
                writer.WriteString(this.GetOperationSignature(operation));
                writer.WriteEndElement();
                writer.WriteElementString("td", this.GetOperationDescription(operation));
                writer.WriteEndElement();
            }

            writer.WriteEndElement(); // </table>
        }

        private string GetOperationDescription(OperationDescription operation)
        {
            DescriptionAttribute[] description = (DescriptionAttribute[])operation.SyncMethod.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (description == null || description.Length == 0 || string.IsNullOrEmpty(description[0].Description))
            {
                return "Operation " + operation.SyncMethod.Name;
            }
            else
            {
                return description[0].Description;
            }
        }

        private string GetOperationSignature(OperationDescription operation)
        {
            StringBuilder sb = new StringBuilder();
            if (operation.SyncMethod != null)
            {
                MethodInfo method = operation.SyncMethod;
                if (method.ReturnType == null || method.ReturnType == typeof(void))
                {
                    sb.Append("void");
                }
                else
                {
                    sb.Append(method.ReturnType.Name);
                }

                sb.Append(' ');
                sb.Append(method.Name);
                sb.Append('(');
                ParameterInfo[] parameters = method.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.AppendFormat("{0} {1}", parameters[i].ParameterType.Name, parameters[i].Name);
                }

                sb.Append(')');
            }
            else
            {
                throw new NotImplementedException("Behavior not yet implemented for async operations");
            }

            return sb.ToString();
        }

        private MessageProperties InitializeMessageProperties()
        {
            MessageProperties result = new MessageProperties();
            HttpResponseMessageProperty httpResponse = new HttpResponseMessageProperty();
            httpResponse.StatusCode = HttpStatusCode.OK;
            httpResponse.Headers[HttpResponseHeader.ContentType] = "text/html";
            result.Add(HttpResponseMessageProperty.Name, httpResponse);
            return result;
        }
    }
}
