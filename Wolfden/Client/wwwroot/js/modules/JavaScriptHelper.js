export function SetProperty(propertyPath, value, object = window) {
    var obj = object;

    for (var i = 0; i < propertyPath.length - 1; i++) {
        obj = obj[propertyPath[i]];
    }

    obj[propertyPath[propertyPath.length - 1]] = value;
}

export function SetFunctionProperty(csObject, method, propertyPath, object = window) {
    var value = () => { csObject.invokeMethodAsync(method); };
    SetProperty(propertyPath, value, object);
}

export function GetProperty(propertyPath, object = window) {
    var obj = object;

    for (var i = 0; i < propertyPath.length; i++) {
        obj = obj[propertyPath[i]];
    }

    return obj;
}

export function InstantiateClass(constructorPath, args) {
    var obj = window;

    for (var i = 0; i < constructorPath.length; i++) {
        obj = obj[constructorPath[i]];
    }

    if (!args)
        args = [];

    args.unshift(obj);
    return ConstructorArgumentShimmer.apply(null, args);
}

function construct(constructor, args) {
    function F() {
        return constructor.apply(this, args);
    }
    F.prototype = constructor.prototype;
    return new F();
}

function ConstructorArgumentShimmer(constructor, a1, a2, a3, a4, a5, a6, a7) {
    switch (arguments.length) {
        case 1:
            return new constructor();
        case 2:
            return new constructor(a1);
        case 3:
            return new constructor(a1, a2);
        case 4:
            return new constructor(a1, a2, a3);
        case 5:
            return new constructor(a1, a2, a3, a4);
        case 6:
            return new constructor(a1, a2, a3, a4, a5);
        case 7:
            return new constructor(a1, a2, a3, a4, a5, a6);
        case 8:
            return new constructor(a1, a2, a3, a4, a5, a6, a7);
    }
};