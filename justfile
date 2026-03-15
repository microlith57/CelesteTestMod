default: refresh_lib build

build:
    dotnet build

_mkdir_lib:
    mkdir -p Source/lib-stripped

_copy_and_strip filename:
    cp ../../{{filename}} Source/lib-stripped
    mono-cil-strip Source/lib-stripped/{{filename}}

refresh_lib: _mkdir_lib
    @just _copy_and_strip Celeste.dll
    @just _copy_and_strip MMHOOK_Celeste.dll
    @just _copy_and_strip FNA.dll
