自制插件步骤：
1. 引用项目PluginTemplate
2. 继承AbstractKeeper等抽象Keeper并进行实现
3. nuget添加包"ILRepack.MSBuild.Task"引用
4. csproj中确保存在以下内容
```
  <Target Name="ILRepack" AfterTargets="Build">
    <PropertyGroup>
      <WorkingDirectory>$(MSBuildThisFileDirectory)bin\$(Configuration)\$(TargetFramework)</WorkingDirectory>
    </PropertyGroup>

    <ItemGroup>
      <AllDll Include="$(WorkingDirectory)\*.dll" ></AllDll>
      <InputAssemblies Include="@(AllDll)" Condition="'%(Filename)' != 'PluginTemplate'" />
    </ItemGroup>

    <ILRepack OutputType="$(OutputType)" MainAssembly="$(AssemblyName).dll" OutputAssembly="$(AssemblyName).dll" InputAssemblies="@(InputAssemblies)" WorkingDirectory="$(WorkingDirectory)" />
  </Target>

```
```
  <PropertyGroup>
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
  </PropertyGroup>
```
5. 插件发布时部署选择【框架依赖】，目标运行时选择【可移植的】