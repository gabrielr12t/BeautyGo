namespace BeautyGo.Contracts.BeautyGoHttp
{
    public class QueryParameterModel
    {
        public QueryParameterModel(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return @$"Params: {Environment.NewLine}
                                \t Key: {Key} | Value: {Value}";
        }
    }
}
