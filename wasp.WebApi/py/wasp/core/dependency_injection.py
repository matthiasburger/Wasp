import builtins

builtins.di_resolver = DiContainer


def resolve(name):
    return builtins.di_resolver.Resolve(name)