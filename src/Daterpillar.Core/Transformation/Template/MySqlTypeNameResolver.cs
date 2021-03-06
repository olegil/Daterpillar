﻿namespace Gigobyte.Daterpillar.Transformation.Template
{
    public sealed class MySqlTypeNameResolver : TypeNameResolverBase
    {
        public MySqlTypeNameResolver()
        {
        }

        public override string GetName(DataType dataType)
        {
            string typeName = dataType.Name;
            switch (typeName)
            {
                case CHAR:
                case VARCHAR:
                    typeName = $"{typeName}({dataType.Scale})";
                    break;

                case DECIMAL:
                    typeName = $"{typeName}({dataType.Scale}, {dataType.Precision})";
                    break;

                default:
                    typeName = TypeNames[typeName];
                    break;
            }

            return typeName.ToUpper();
        }
    }
}