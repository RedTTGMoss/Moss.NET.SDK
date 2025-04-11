# Moss.NET.SDK

[![runs_on](https://img.shields.io/badge/runs_on-Extism-4c30fc.svg?subject=runs_on&status=Extism&color=4c30fc)](https://modsurfer.dylibso.com/module?hash=fa39db232381e9de32a6e5b863edf5dc1552dc0e63682ad655cc72c2e042f9fa)
![NuGet Version](https://img.shields.io/nuget/v/Moss.NET.SDK)
![NuGet Downloads](https://img.shields.io/nuget/dt/Moss.NET.SDK)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
![Discord](https://img.shields.io/discord/455738571186241536)
![Libraries.io SourceRank](https://img.shields.io/librariesio/sourcerank/nuget/Moss.NET.SDK)

A library that exposes all functions and data structures to build [moss](https://github.com/RedTTGMoss/moss-desktop) extensions more easily.
The sdk provides an idiomatic wrapper around the [host functions](https://redttg.gitbook.io/moss/extensions/host-functions).
You can find a full overview of all functionalities the sdk provides in the [wiki](https://github.com/RedTTGMoss/Moss.NET.SDK/wiki).

# Getting Started

1. Install .Net 8
2. Install the custom template with `dotnet new install Moss.Net.Sdk.Templates`
3. Create a new project with `dotnet new moss-plugin`

You can find also an example plugin that uses all functionalities of the sdk [here](https://github.com/RedTTGMoss/Moss.NET.SDK/tree/main/src/SamplePlugin).   

# Generating Notebooks

To generate epub notebooks the sdk has integrated a changed version of the library [EpubSharp](https://github.com/asido/EpubSharp). If you want to generate pdf notebooks you can use [PdfPig](https://github.com/UglyToad/PdfPig).
