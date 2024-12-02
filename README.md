[![CI](https://github.com/OuterlimitsTech/olt-automapper/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-automapper/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-automapper&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-automapper)


# OLT.DataAdapters.AutoMapper

## Overview

`OLT.DataAdapters.AutoMapper` is a library that integrates AutoMapper with the OLT DataAdapters framework. It provides utilities and extensions to simplify the configuration and usage of AutoMapper within OLT-based applications.

## Features

- Seamless integration with AutoMapper.
- Easy configuration of AutoMapper profiles and mappings.
- Includes utilities for assembly scanning for maps.

## Getting Started

### Installation

To install the [![Nuget](https://img.shields.io/nuget/v/OLT.DataAdapters.AutoMapper)](https://www.nuget.org/packages/OLT.DataAdapters.AutoMapper) package, add the following package reference to your project file:

```bash
dotnet add package OLT.DataAdapters.AutoMapper
```

To install the [![Nuget](https://img.shields.io/nuget/v/OLT.Extensions.DependencyInjection.AutoMapper)](https://www.nuget.org/packages/OLT.Extensions.DependencyInjection.AutoMapper) extensions package, add the following package reference to your project file:

```bash
dotnet add package OLT.Extensions.DependencyInjection.AutoMapper
```


### Usage

#### OLT Adapter Resolver and adapters for AutoMapper

```csharp

// Inject IOltAdapterResolver 


// Checks to see if can project IQueryable
adapterResolver.CanProjectTo<PersonEntity, PersonModel>();  

//Simple Map
var person = adapterResolver.Map<PersonEntity, PersonModel>(entity, new PersonModel());


var queryable = Context.People.GetAll();
var records = adapterResolver.ProjectTo<PersonEntity, PersonModel>(queryable);

```

#### Simple Adapater

```csharp

public class PersonEntityToPersonModelAdapter : OltAdapter<PersonEntity, PersonModel>
{
	public override void Map(PersonEntity source, PersonModel destination)
	{
		destination.Name = new PersonName
		{
			First = source.FirstName,
			Last = source.LastName,
		};
	}

	public override void Map(PersonModel source, PersonEntity destination)
	{
		destination.FirstName = source.Name.First;
		destination.LastName = source.Name.Last;
	}
}

```

