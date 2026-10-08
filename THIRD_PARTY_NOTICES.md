# Third-Party Notices

ResearchAtlas is licensed under the [GNU Affero General Public License v3.0](LICENSE).
This document identifies the third-party NuGet packages used by the repository and
their declared licenses. It does not grant any rights in third-party software.

Package versions and transitive dependencies are resolved during restore. Before
distributing a build, retain the applicable copyright notices and license texts
included with every resolved package in that build.

## Runtime dependencies

The following direct runtime dependencies are declared by the application projects.
Their license expressions or bundled license files were reviewed from the restored
NuGet packages.

| Package | Version | License |
| --- | ---: | --- |
| Autofac | 9.3.4 | MIT |
| Autofac.Extensions.DependencyInjection | 11.0.2 | MIT |
| Azure.Storage.Blobs | 12.30.1 | MIT |
| BouncyCastle.Cryptography | 2.7.0 | MIT |
| ClosedXML | 0.105.1 | MIT |
| CsvHelper | 33.1.0 | MS-PL OR Apache-2.0 |
| FluentEmail.Smtp | 3.0.2 | MIT |
| FluentValidation | 12.1.1 | Apache-2.0 |
| GraphQL | 8.8.5 | MIT |
| Hangfire.SqlServer | 1.8.25 | LGPL-3.0-or-later |
| HtmlSanitizer | 9.2.1039 | MIT |
| Humanizer.Core | 3.0.10 | MIT |
| Humanizer.Core.ca | 3.0.10 | MIT |
| Humanizer.Core.es | 3.0.10 | MIT |
| itext | 9.8.0 | AGPL-3.0-or-later or commercial license |
| Microsoft.AspNetCore.OpenApi | 10.0.12 | MIT |
| Microsoft.Azure.SignalR | 1.33.1 | MIT |
| Microsoft.EntityFrameworkCore.SqlServer | 10.0.12 | MIT |
| Microsoft.Extensions.Caching.Memory | 10.0.12 | MIT |
| Microsoft.Extensions.Caching.StackExchangeRedis | 10.0.12 | MIT |
| Microsoft.Extensions.Hosting | 10.0.12 | MIT |
| Microsoft.Extensions.Http.Resilience | 10.10.0 | MIT |
| Microsoft.Extensions.Logging.Abstractions | 10.0.12 | MIT |
| NCalc | 7.2.0 | MIT |
| Newtonsoft.Json | 13.0.4 | MIT |
| Scriban | 7.5.0 | BSD-2-Clause |
| Serilog.AspNetCore | 10.0.0 | Apache-2.0 |
| Serilog.Sinks.Console | 6.1.1 | Apache-2.0 |
| SSH.NET | 2026.0.0 | MIT |

### Notable transitive dependencies

| Package family or package | Version | License |
| --- | ---: | --- |
| ClosedXML.Parser | 2.0.0 | MIT |
| DocumentFormat.OpenXml | 3.1.1 | MIT |
| DocumentFormat.OpenXml.Framework | 3.1.1 | MIT |
| ExcelNumberFormat | 1.1.0 | MIT |
| GraphQL.Analyzers | 8.8.5 | MIT |
| GraphQL-Parser | 9.5.0 | MIT |
| Hangfire.Core | 1.8.25 | LGPL-3.0-or-later |
| itext.commons | 9.8.0 | AGPL-3.0-or-later or commercial license |
| Microsoft, Azure, and System runtime packages | Various | MIT, unless the package's bundled notice states otherwise |
| SixLabors.Fonts | 1.0.0 | Apache-2.0 |

## Development and test dependencies

The following packages are used only to build or test ResearchAtlas and are not
intended to be included in production deployment artifacts.

| Package | Version | License |
| --- | ---: | --- |
| AutoFixture.Xunit3 | 4.19.0 | MIT |
| Bogus | 35.6.5 | MIT |
| NSubstitute | 6.2.0 | BSD-3-Clause |
| xunit.v3.mtp-v2 | 4.0.1 | Apache-2.0 |

## License selection and distribution

ResearchAtlas uses iText under its AGPL-3.0-or-later option. Distributors that do
not wish to comply with the AGPL terms for iText must obtain an appropriate
commercial license directly from iText.

The dependency set must not include packages whose terms conflict with AGPL-3.0 or
with the intended distribution model. New direct dependencies and their resolved
transitive dependencies must be reviewed before being added to a release.
