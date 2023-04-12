using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace shared
{
    /**
     * Classes that extend ASerializable can (de)serialize themselves into/out of a Packet instance. 
     * See the classes in the protocol package for an example. 
     * This base class provides a ToString method for simple (and slow) debugging.
     */
    public abstract class ASerializable
    {
        //Abstract class because Unity doesn't allow to serialize interfaces in the inspector
        abstract public void Serialize(Packet pPacket);
        abstract public void Deserialize(Packet pPacket);

        public override string ToString()
        {
            //Modifies the string instead of creating the new ones, used to prevent taking too much memory
            StringBuilder builder = new StringBuilder();

            builder.Append("\n" + GetType().Name + ":");
            builder.Append("\n----------------------------------------------------------");

            IEnumerable<FieldInfo> publicFields = GetType().GetFields().Where(f => f.IsPublic);

            foreach (FieldInfo field in publicFields)
            {
                object value = field.GetValue(this);
                if (value is ICollection)
                {
                    ICollection collection = value as ICollection;
                    foreach (object item in collection) builder.Append(item.ToString());
                }
                else
                {
                    builder.Append(String.Format("\nName: {0} \t\t\t Value: {1}", field.Name, value) + "");
                }
            }

            builder.Append("\n----------------------------------------------------------");

            return builder.ToString();
        }
    }
}
