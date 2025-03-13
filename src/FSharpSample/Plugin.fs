module SamplePlugin.Plugin

open System.Runtime.CompilerServices
open Moss.NET.Sdk

type SampleExtension() =
        inherit MossExtension()

        static let logger: LoggerInstance = Log.GetLogger<SampleExtension>()

        [<ModuleInitializer>]
        static member ModInit() = SampleExtension.Init<SampleExtension>()

        static member Main() = ()

        override this.Register(state: MossState)  =
            logger.Info("Hello world from f#")

        override this.Unregister() = ()