using Autofac;
using ResearchAtlas.Application;
using ResearchAtlas.Infrastructure.Cache.Memory;
using ResearchAtlas.Infrastructure.Cache.Redis;
using ResearchAtlas.Infrastructure.Identity.EntraId;
using ResearchAtlas.Infrastructure.Persistence.SqlServer;
using ResearchAtlas.Infrastructure.Remote.Http;
using ResearchAtlas.Infrastructure.Storage.AzureBlob;
using ResearchAtlas.Infrastructure.Storage.FileSystem;
using ResearchAtlas.Infrastructure.Templating;

using ResearchAtlas.Infrastructure.Identity.OpenIdConnect;
using ResearchAtlas.Infrastructure.Identity.Saml;
using ResearchAtlas.Infrastructure.Identity.BuiltIn;
using ResearchAtlas.Infrastructure.Messaging;
using ResearchAtlas.Infrastructure.Remote.Sftp;
using ResearchAtlas.Infrastructure.Cryptography;
using ResearchAtlas.Infrastructure.ExpressionEvaluation;
using ResearchAtlas.Infrastructure.Exporting.Excel;
using ResearchAtlas.Infrastructure.Pdf.DigitalSignature;
using ResearchAtlas.Infrastructure.Exporting.Csv;
using ResearchAtlas.Infrastructure.Exporting.Pdf;
using ResearchAtlas.Infrastructure.JobScheduling.Hangfire;
using ResearchAtlas.Infrastructure.Messaging.Email;

namespace ResearchAtlas.DependencyInjection;

public sealed class ResearchAtlasContainerModule : Module
{
    private readonly bool isDevelopment;

    public ResearchAtlasContainerModule(bool isDevelopment)
    {
        this.isDevelopment = isDevelopment;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<ApplicationModule>();
        
        builder.RegisterModule<PersistenceSqlServerModule>();
        
        if (isDevelopment)
        {
            builder.RegisterModule<StorageFileSystemModule>();
        }
        else
        {
            builder.RegisterModule<StorageAzureBlobModule>();
        }        


        if (isDevelopment)
        {
            builder.RegisterModule<CacheMemoryModule>();
        }
        else
        {
            builder.RegisterModule<CacheRedisModule>();
        }

        builder.RegisterModule<TemplatingModule>();
        builder.RegisterModule<ExportingExcelModule>();
        builder.RegisterModule<ExportingCsvModule>();
        builder.RegisterModule<ExportingPdfModule>();
        builder.RegisterModule<PdfDigitalSignatureModule>();
        builder.RegisterModule<CryptographyModule>();
        builder.RegisterModule<JobSchedulingHangfireModule>();
        builder.RegisterModule<ExpressionEvaluationModule>();

        builder.RegisterModule<IdentityEntraIdModule>();
        builder.RegisterModule<IdentityOpenIdConnectModule>();
        builder.RegisterModule<IdentitySamlModule>();
        builder.RegisterModule<IdentityBuiltInModule>();

        builder.RegisterModule<RemoteHttpModule>();
        builder.RegisterModule<RemoteSftpModule>();        
        builder.RegisterModule<MessagingEmailModule>();
    }
}