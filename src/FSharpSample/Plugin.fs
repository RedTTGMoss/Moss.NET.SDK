module SamplePlugin.Plugin

open System.Runtime.InteropServices
open Moss.NET.Sdk

type SampleExtension() =
        inherit MossExtension()

        static let logger: LoggerInstance = Log.GetLogger<SampleExtension>()

        [<UnmanagedCallersOnly(EntryPoint = "moss_extension_register")>]
        static member Register() : uint64 =
            SampleExtension.Init<SampleExtension>()
            0UL

        static member Main() = ()

        override this.Register(state: MossState)  =
            logger.Info("Hello world from f#")

        override this.Unregister() = ()