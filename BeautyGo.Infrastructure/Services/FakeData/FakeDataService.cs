using Bogus;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.FakeData;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications.UserRoles;
using BeautyGo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using BeautyGo.Domain.Entities.Businesses;

public class FakeDataService : IFakeDataService
{
    #region Fields

    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Business> _storeRepository;
    private readonly IBaseRepository<UserRole> _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region Ctor

    public FakeDataService(
        IBaseRepository<User> userRepository,
        IBaseRepository<Business> storeRepository,
        IBaseRepository<UserRole> userRoleRepository,
        IUnitOfWork unitOfWork,
        IServiceProvider serviceProvider)
    {
        _userRepository = userRepository;
        _storeRepository = storeRepository;
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
        _serviceProvider = serviceProvider;
    }

    #endregion

    #region Utilities

    public static string GenerateCPF()
    {
        var random = new Random();
        var cpfNumeros = new int[9];
        for (int i = 0; i < 9; i++)
            cpfNumeros[i] = random.Next(0, 10);

        int primeiroDigito = CalculateDigit(cpfNumeros, multiplicadores: [10, 9, 8, 7, 6, 5, 4, 3, 2]);
        cpfNumeros = cpfNumeros.Append(primeiroDigito).ToArray();

        int segundoDigito = CalculateDigit(cpfNumeros, multiplicadores: [11, 10, 9, 8, 7, 6, 5, 4, 3, 2]);
        cpfNumeros = cpfNumeros.Append(segundoDigito).ToArray();

        string cpfFormatado = string.Join("", cpfNumeros.Select(n => n.ToString()));
        return Convert.ToUInt64(cpfFormatado).ToString(@"000\.000\.000\-00");
    }

    public static string GenerateCnpj()
    {
        var random = new Random();
        var cnpjNumeros = new int[12];
        for (int i = 0; i < 8; i++)
        {
            cnpjNumeros[i] = random.Next(0, 10);
        }

        cnpjNumeros[8] = 0;
        cnpjNumeros[9] = 0;
        cnpjNumeros[10] = 0;
        cnpjNumeros[11] = 1;

        int primeiroDigito = CalculateDigit(cnpjNumeros, multiplicadores: new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });
        cnpjNumeros = cnpjNumeros.Append(primeiroDigito).ToArray();

        int segundoDigito = CalculateDigit(cnpjNumeros, multiplicadores: new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });
        cnpjNumeros = cnpjNumeros.Append(segundoDigito).ToArray();

        string cnpjFormatado = string.Join("", cnpjNumeros.Select(n => n.ToString()));
        return Convert.ToUInt64(cnpjFormatado).ToString(@"00\.000\.000\/0000\-00");
    }

    private static int CalculateDigit(int[] cpfNumeros, int[] multiplicadores)
    {
        int soma = 0;
        for (int i = 0; i < multiplicadores.Length; i++)
        {
            soma += cpfNumeros[i] * multiplicadores[i];
        }

        int resto = soma % 11;
        return (resto < 2) ? 0 : 11 - resto;
    }

    public static string GenerateCode(int tamanho = 6)
    {
        const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();

        var codigoAleatorio = new string(Enumerable.Repeat(caracteres, tamanho)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return codigoAleatorio;
    }

    #endregion

    public async Task SeedAsync()
    {
        //if (await _storeRepository.Query().AnyAsync())
        //    return;

        var faker = new Faker("pt_BR");

        var random = new Random();

        var ownerRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.OWNER);
        var customerRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.CUSTOMER);
        //var employeeRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.EMPLOYEE);

        var ownerRole = await _userRoleRepository.GetFirstOrDefaultAsync(ownerRoleSpecification);

        //#region Owners

        //var salt = EncryptionHelper.CreateSaltKey(10);
        //var pass = EncryptionHelper.CreatePasswordHash("12345", salt);

        //var owners = new ConcurrentBag<StoreOwner>();
        //Parallel.For(0, 50_000, i =>
        //{
        //    var owner = new Faker<StoreOwner>()
        //        .RuleFor(p => p.Id, f => Guid.NewGuid())
        //        .RuleFor(p => p.FirstName, f => f.Name.FirstName())
        //        .RuleFor(p => p.LastName, f => f.Name.LastName())
        //        .RuleFor(p => p.Email, f => f.Internet.Email())
        //        .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber())
        //        .RuleFor(p => p.Password, pass) // Supondo que 'pass' seja uma variável definida anteriormente
        //        .RuleFor(p => p.PasswordSalt, salt) // Supondo que 'salt' seja uma variável definida anteriormente
        //        .RuleFor(p => p.Cpf, GenerateCPF()) // Supondo que 'GenerateCPF()' é um método que gera CPF válido
        //        .RuleFor(p => p.IsActive, true)
        //        .RuleFor(p => p.EmailConfirmed, true)
        //        .RuleFor(p => p.UserRoles, (f, so) => so.UserRoles = new List<UserUserRoleMapping> { new UserUserRoleMapping { UserRoleId = ownerRole.Id } }) // Supondo que 'ownerRole' está disponível

        //        .Generate();

        //    owners.Add(owner); // Adiciona na coleção thread-safe
        //});

        //await _userRepository.InsertRangeAsync(owners);
        //await _unitOfWork.SaveChangesAsync();

        //#endregion

        //#region Stores

        //var stores = new List<Store>();
        //for (int i = 0; i < 10_000; i++)
        //{
        //    stores.Add(new Faker<Store>()
        //        .RuleFor(s => s.Id, f => Guid.NewGuid())
        //        .RuleFor(s => s.Name, f => f.Company.CompanyName())
        //        .RuleFor(s => s.Description, f => f.Company.Bs())
        //        .RuleFor(s => s.CNPJ, GenerateCnpj())
        //        .RuleFor(s => s.Host, f => f.Internet.DomainName())
        //        .RuleFor(s => s.Phone, f => f.Phone.PhoneNumber())
        //        .RuleFor(s => s.Code, f => Guid.NewGuid())
        //        .RuleFor(s => s.OwnerId, owners.ToList()[faker.Random.Int(0, owners.Count - 1)].Id)
        //        .RuleFor(s => s.CustomerLoginRequired, f => f.Random.Bool())
        //        .RuleFor(s => s.Deleted, f => false)
        //        .Generate());
        //}

        //await _storeRepository.InsertRangeAsync(stores);
        //await _unitOfWork.SaveChangesAsync();

        //#endregion

        //#region Store Table

        //var storeTables = new ConcurrentBag<StoreTable>();
        //Parallel.For(0, 15_000, i =>
        //{
        //    var storeTable = new Faker<StoreTable>()
        //        .RuleFor(s => s.Id, f => Guid.NewGuid())
        //        .RuleFor(s => s.IsBusy, f => f.Random.Bool())
        //        .RuleFor(s => s.QRCode, GenerateCode()) // Supondo que 'GenerateCode()' é um método que gera um código
        //        .RuleFor(s => s.Number, f => f.Random.Int(1, 30).ToString())
        //        .RuleFor(s => s.Deleted, false)
        //        .RuleFor(s => s.StoreId, stores[faker.Random.Int(0, stores.Count - 1)].Id)
        //        .Generate();

        //    storeTables.Add(storeTable); // Adiciona na coleção thread-safe
        //});

        //await _storeTableRepository.InsertRangeAsync(storeTables);
        //await _unitOfWork.SaveChangesAsync();

        //#endregion

        //#region Order Source

        //var orderSources = new ConcurrentBag<TableOrder>();
        //Parallel.For(0, 20_000, i =>
        //{
        //    var orderSource = new Faker<TableOrder>()
        //        .RuleFor(s => s.Id, f => Guid.NewGuid())
        //        .RuleFor(s => s.StoreTableId, storeTables.ToList()[faker.Random.Int(0, storeTables.Count - 1)].Id)
        //        .Generate();

        //    orderSources.Add(orderSource); // Adiciona na coleção thread-safe
        //});

        //await _orderSourceRepository.InsertRangeAsync(orderSources);
        //await _unitOfWork.SaveChangesAsync();

        //#endregion

        //#region Order 

        //var orders = new ConcurrentBag<Order>();
        //Parallel.For(0, 5_000_000, i =>
        //{
        //    var order = new Faker<Order>()
        //        .RuleFor(s => s.Id, f => Guid.NewGuid())
        //        .RuleFor(s => s.Price, f => f.Random.Decimal(5, 200))
        //        .RuleFor(s => s.StoreId, stores[faker.Random.Int(0, stores.Count - 1)].Id)
        //        .RuleFor(s => s.OrderSourceId, orderSources.ToList()[faker.Random.Int(0, orderSources.Count - 1)].Id)
        //    .Generate();

        //    orders.Add(order);  
        //});

        //await _orderRepository.InsertRangeAsync(orders);
        //await _unitOfWork.SaveChangesAsync();

        //#endregion

        //#region Catalog

        //var categories = new ConcurrentBag<ProductCategory>();
        //Parallel.For(0, 30_000, i =>
        //{
        //    var category = new Faker<ProductCategory>()
        //        .RuleFor(c => c.Id, f => Guid.NewGuid())
        //        .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
        //        .RuleFor(c => c.StoreId, f => stores[faker.Random.Int(0, stores.Count - 1)].Id)
        //        .Generate();

        //    categories.Add(category); // Adiciona na coleção thread-safe
        //});

        //await _productCategoryRepository.InsertRangeAsync(categories);
        //await _unitOfWork.SaveChangesAsync();

        //var subcategories = new ConcurrentBag<ProductSubCategory>();
        //Parallel.ForEach(categories, category =>
        //{
        //    var subcategory = new Faker<ProductSubCategory>()
        //        .RuleFor(sc => sc.Id, f => Guid.NewGuid())
        //        .RuleFor(sc => sc.Name, f => f.Commerce.Department())
        //        .RuleFor(sc => sc.CategoryId, f => category.Id)
        //        .Generate();

        //    subcategories.Add(subcategory); // Adiciona na coleção thread-safe
        //});

        //await _productSubCategoryRepository.InsertRangeAsync(subcategories);
        //await _unitOfWork.SaveChangesAsync();

        //var products = new ConcurrentBag<Product>();
        //Parallel.ForEach(subcategories, subCategory =>
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        var product = new Faker<Product>()
        //            .RuleFor(p => p.Id, f => Guid.NewGuid())
        //            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        //            .RuleFor(p => p.Image, f => f.Internet.Url())
        //            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 100))
        //            .RuleFor(p => p.ShortDescription, f => f.Commerce.ProductDescription())
        //            .RuleFor(p => p.FullDescription, f => f.Lorem.Paragraphs(2))
        //            .RuleFor(p => p.SubCategoryId, f => subCategory.Id)
        //            .RuleFor(p => p.Deleted, f => false)
        //            .Generate();

        //        products.Add(product); // Adiciona na coleção thread-safe
        //    }
        //});

        //await _productRepository.InsertRangeAsync(products);
        //await _unitOfWork.SaveChangesAsync();

        //#endregion

    }
}
