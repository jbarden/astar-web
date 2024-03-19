# AStar.Clean.V1

## Introduction

The AStar.Clean.V1 project is designed as a starting point for a Blazor Application with a fully RESTful API backing it. Whilst the project is under active development,
the intent is to demonstrate the relevant best practices in both a meaningful and re-usable way.

## Testing

As with any project, testing is a key component of this project and placeholder projects have been created to both test the solution (where applicable) and
to act as a starter for / reminder of the types of tests that should be implemented.

## Architecture

The starting point of this project was taken from [Ardalis Clean Architecture](https://github.com/ardalis/CleanArchitecture) but with two project renames:

1. CleanArchitecture.Core > AStar.Clean.V1.Services
1. CleanArchitecture.SharedKernel > AStar.Clean.V1.DomainModel

These changes were made as the new names more closely represent their intended usage. Opinions will, no doubt, vary - please feel free to revert
the renames if the original project suffixes more closely fit your project structure.

## Architecture Tests

In the tests\architecture solution (and actual) folder, there is an AStar.Clean.V1.Architecture.Tests project that, as you can probably deduce from the name,
is intended to enforce the project structure and hierarchy - specifically, the higher-level projects can depend on lower-level projects but, not the other
way around.
