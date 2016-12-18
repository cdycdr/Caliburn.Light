# History

At the beginning of 2012 I did my first steps with WPF and MVVM (quite late I know). 
After reading [Sams Teach Yourself WPF in 24 Hours](http://www.amazon.com/Sams-Teach-Yourself-WPF-Hours/dp/0672329859) by Rob Eisenberg I began using [Caliburn.Micro](https://github.com/caliburn-micro/caliburn.micro).
His framework was really helpful to achieve project goals at this time. Later in July 2012 I began contributing to Caliburn.Micro. First some small fixes only, later some complete features.

As the project proceeded there came up some issues with the framework and how it was used. In our team we realized that the conventions that Caliburn.Micro makes so powerful makes code + XAML harder to read and change. Often something was broken because of just a refactoring as the name change broke the convention.

In April 2014 I started my project **Caliburn.Light**. Initially it was a fork of Caliburn.Micro with support for WPF and WinRT only.
Conventions where removed and instead `ICommand` support was added.

And the rest is history!
