﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace CustomContractBehavior
{
    public class MyNewSerializerContractBehaviorAttribute : Attribute, IContractBehavior
    {
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            this.ReplaceSerializerOperationBehavior(contractDescription);
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            this.ReplaceSerializerOperationBehavior(contractDescription);
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            foreach (OperationDescription operation in contractDescription.Operations)
            {
                foreach (MessageDescription message in operation.Messages)
                {
                    if (message.Direction == MessageDirection.Input)
                    {
                        foreach (MessagePartDescription part in message.Body.Parts)
                        {
                            this.ValidateMessagePartDescription(part);
                        }
                    }
                    else
                    {
                        this.ValidateMessagePartDescription(message.Body.ReturnValue);
                    }

                    foreach (MessageHeaderDescription header in message.Headers)
                    {
                        this.ValidateCustomSerializableType(header.Type);
                    }
                }
            }
        }

        private void ValidateMessagePartDescription(MessagePartDescription part)
        {
            if (part != null)
            {
                this.ValidateCustomSerializableType(part.Type);
            }
        }

        private void ValidateCustomSerializableType(Type type)
        {
            if (typeof(ICustomSerializable).IsAssignableFrom(type))
            {
                if (!type.IsPublic)
                {
                    throw new InvalidOperationException("Custom serialization is supported in public types only");
                }

                ConstructorInfo defaultConstructor = type.GetConstructor(new Type[0]);
                if (defaultConstructor == null)
                {
                    throw new InvalidOperationException("Custom serializable types must have a public, parameterless constructor");
                }
            }
        }

        private void ReplaceSerializerOperationBehavior(ContractDescription contract)
        {
            foreach (OperationDescription od in contract.Operations)
            {
                for (int i = 0; i < od.Behaviors.Count; i++)
                {
                    DataContractSerializerOperationBehavior dcsob = od.Behaviors[i] as DataContractSerializerOperationBehavior;
                    if (dcsob != null)
                    {
                        od.Behaviors[i] = new MyNewSerializerOperationBehavior(od);
                    }
                }
            }
        }

        class MyNewSerializerOperationBehavior : DataContractSerializerOperationBehavior
        {
            public MyNewSerializerOperationBehavior(OperationDescription operation) : base(operation) { }
            public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns, IList<Type> knownTypes)
            {
                return new MyNewSerializer(type, base.CreateSerializer(type, name, ns, knownTypes));
            }

            public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name, XmlDictionaryString ns, IList<Type> knownTypes)
            {
                return new MyNewSerializer(type, base.CreateSerializer(type, name, ns, knownTypes));
            }
        }
    }
}
