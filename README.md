> Archived – Now maintained in the Netherlands3D Main Repository; OpenUPM updates suspended.

# Masking

This package contains tools to do runtime masking of shaders via global shader properties using a moveable dome 

## Installing

This package is provided through OpenUPM, to install it using the CLI you can perform the following:

```bash
$ openupm add eu.netherlands3d.masking
```

or, you have to add `https://package.openupm.com` as a scoped registry with, at least, the following scopes:

- `eu.netherlands3d`

## Usage

Add the MaskingDomeSpawner prefab to your scene.

Click somewhere in the world to move the dome directly to that position.

Drag the dome, or click on a new position to move it around.

Drag the scaling handle to resize the dome. (the initial placement scale will be based on camera distance)  

Any material using the 'SphereMasking' SubGraph will be masked out by the dome. 

##### Shaders

If you want to customise or create your own shader the following global shader variables* can be used to set the spherical mask radius and world position:

- _SphericalMaskRadius (Float)
- _SphericalMaskPosition (Vector3)

*Using ShaderGraph, you can untick the 'Exposed' box to make a variable Global (instead of serialized in the editor)

You can use the 'SphereMasking' SubGraph inside your own shader to use the premade spherical masking graph or you can create your own custom shader using the global shader variables as input.

# Repository Archived

This repository has been **archived** and is no longer maintained independently.

The contents of this package have been merged into the  
**[Netherlands3D Main Repository](https://github.com/Netherlands3D/twin)**  
into the `Packages` folder.

## Current Location

The latest version and future updates of this package will be maintained inside the Netherlands3D main repository as part of an effort to simplify and unify our development workflow.

## Why was this repository archived?

To streamline development and reduce overhead, the Netherlands3D packages are being integrated into the main repository.  
This approach allows for better coordination between packages and features, while we continue to evaluate whether a full monorepo setup (including versioning) is desirable in the future.

## Where to contribute

Please open issues and pull requests in the [Netherlands3D Main Repository](https://github.com/Netherlands3D/twin).

## Publication status

Updates to this package on **OpenUPM** are **suspended until further notice**.  
Future releases will be managed from the main repository once the new development flow has been finalized.

## Historical reference

This repository remains available in read-only mode as a historical record of its standalone development.

