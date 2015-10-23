# Welcome to NJali
Jali cross-platform service specification and execution context. NJali is the .NET implementation.

[comment]: # (This document is modeled after the NLog README.md file.)

AppVeyor: [![Build status](https://ci.appveyor.com/api/projects/status/tee4xl00svt85g1x?svg=true)](https://ci.appveyor.com/project/kenbrubaker/njali)

Jali is a polyglot family of libraries for implementing distributed
software. The focus is around implementing robust routines in the context 
of an explicit execution context and service specification. These features 
permit rich microservice features such as

* Self-documenting services
* Hypermedia-driven feature discovery
* Client-driven contracts
* Global scaling and monitoring
* Non-destructive production testing
* Language and platform agnostic
* Infrastructure agnostic
* Many more!

Jali-conforming components are implemented in several languages. NJali represents those written in .NET.

## Packages & Status
NJali consists of multiple packages.

| Package | Purpose | Build Status | NuGet |
| :-- | :-- | :-- | :-- |
| Jali.Core | Shared objects and utilities | [![Build status](https://ci.appveyor.com/api/projects/status/tee4xl00svt85g1x?svg=true)](https://ci.appveyor.com/project/kenbrubaker/njali)| |
| Jali | Service method execution context interface | [![Build status](https://ci.appveyor.com/api/projects/status/tee4xl00svt85g1x?svg=true)](https://ci.appveyor.com/project/kenbrubaker/njali)| |
| Jali.Serve | Service specification, definition, and protocol | [![Build status](https://ci.appveyor.com/api/projects/status/tee4xl00svt85g1x?svg=true)](https://ci.appveyor.com/project/kenbrubaker/njali)| |
| Jali.Serve.Server | Service execution host library | [![Build status](https://ci.appveyor.com/api/projects/status/tee4xl00svt85g1x?svg=true)](https://ci.appveyor.com/project/kenbrubaker/njali)| |
| Jali.Serve.AspNet.Mvc | ASP.NET MVC integration for Jali.Serve | [![Build status](https://ci.appveyor.com/api/projects/status/tee4xl00svt85g1x?svg=true)](https://ci.appveyor.com/project/kenbrubaker/njali)| |

## Questions, bug reports or feature requests?
Do you have feature requests, questions or would you like to report a bug? Please post them on the [issue list](https://github.com/Latticework/njali/issues) and follow [these guidelines](CONTRIBUTING.md).


## Contributing
As the current Latticework team is a small team, we cannot fix every bug or implement every feature on our own. So contributions are really appreciated!

## License
Jali is open source software, licensed under the terms of the MIT license. 
See [LICENSE](LICENSE) for details.