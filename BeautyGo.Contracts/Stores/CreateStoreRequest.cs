namespace BeautyGo.Contracts.Stores
{
    public class CreateStoreRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string CNPJ { get; set; }

        public bool CustomerLoginRequired { get; set; }

        //ADICIONAR PROPRIEDADES DE FOTOS
    }
}
