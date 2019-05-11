export DOTNET_ROOT=$HOME/dotnet
export PATH=$HOME/dotnet:$PATH
export MSBuildSDKsPath="${DOTNET_ROOT}/sdk/$(${DOTNET_ROOT}/dotnet --version)/Sdks"
