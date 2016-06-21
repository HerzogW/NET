declare module TypeDocs {
    /**
    * Input for the generator to process.
    */
    interface GeneratorInput {
        /**
        * The TypeScript source to parse.
        */
        sourceText: string;
        /**
        * True if the data is a declaration file; else false.
        */
        isDeclaration: boolean;
        /**
        * The name of the source file corresponding to the data.
        */
        sourceFileName: string;
    }
}
declare module TypeDocs {
    /**
    * Options used for controlling how documentation generation happens.
    */
    interface GeneratorOptions {
        /**
        * If true, then all entities starting with underscore are considered private.
        */
        underscoreIsPrivate?: boolean;
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes common to all syntax elements.
    */
    class Element {
        /**
        * The name of the element.
        */
        public name: string;
        /**
        * The description of the element.
        */
        public description: string;
        /**
        * The type of the element.
        */
        public elementType: ElementType;
        /**
        * The display text for the type of the element.
        */
        public elementTypeText: string;
        /**
        * True if element is public; else false.
        */
        public isPublic: boolean;
        /**
        * True if element is private; else false.
        */
        public isPrivate: boolean;
        /**
        * True if element is public through explicit modifier; else false.
        */
        public isPublicExplicit: boolean;
        /**
        * True if element is private; else false.
        */
        public isStatic: boolean;
        /**
        * Creates a new instance of the DocBase class.
        */
        constructor();
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes common to all syntax elements.
    */
    class ContainerElement extends Element {
        /**
        * The children of the current element.
        */
        public items: Element[];
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes common to all parameter and property elements.
    */
    class ParameterPropertyBase extends Element {
        /**
        * The type of the element.
        */
        public type: string;
        /**
        * True if the element is optional; else false.
        */
        public optional: boolean;
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes common to all class and interface elements.
    */
    class ClassInterfaceBase extends ContainerElement {
        /**
        * The name of the element with generic type parameters.
        */
        public nameWithParameters: string;
        /**
        * The constructor of the element.
        */
        public ctor: Function;
        /**
        * The type parameters of the element.
        */
        public typeParameters: TypeParameter[];
        /**
        * The indexer of the element.
        */
        public indexer: Indexer;
        /**
        * The extends clause of the element.
        */
        public extendsClause: string;
        /**
        * The implements clause of the element.
        */
        public implementsClause: string;
        /**
        * Creates a new instance of the element.
        */
        constructor();
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes of a TypeScript class.
    */
    class Class extends ClassInterfaceBase {
        /**
        * Creates a TypeScript class element.
        */
        constructor();
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes of a TypeScript indexer.
    */
    class Indexer extends ParameterPropertyBase {
        /**
        * Creates a TypeScript indexer element.
        */
        constructor();
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes of a TypeScript enum.
    */
    class Enum extends ContainerElement {
        /**
        * Creates a TypeScript enum element.
        */
        constructor();
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines a TypeScript enum value.
    */
    class EnumValue extends Element {
        /**
        * Creates a TypeScript enum value element.
        */
        constructor();
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes of a TypeScript function.
    */
    class Function extends Element {
        /**
        * The parameters of the function.
        */
        public parameters: Parameter[];
        /**
        * The return value of the function.
        */
        public returns: Parameter;
        /**
        * Creates a TypeScript function element.
        */
        constructor();
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes of a TypeScript interface.
    */
    class Interface extends ClassInterfaceBase {
        /**
        * Creates a TypeScript interface element.
        */
        constructor();
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes of a TypeScript module.
    */
    class Module extends ContainerElement {
        /**
        * The parent module.
        */
        public parent: Module;
        /**
        * The full name of the module.
        */
        public fullName: string;
        /**
        * Creates the module element.
        */
        constructor();
        /**
        * Clones the module.
        *
        * @return The cloned module.
        */
        public clone(): Module;
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes of a TypeScript parameter.
    */
    class Parameter extends ParameterPropertyBase {
        /**
        * Creates a TypeScript parameter element.
        */
        constructor();
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes of a TypeScript type parameter.
    */
    class TypeParameter extends ClassInterfaceBase {
        /**
        * Creates a TypeScript type parameter element.
        */
        constructor();
    }
}
declare module TypeDocs.Syntax {
    /**
    * Defines the attributes of a TypeScript variable.
    */
    class Variable extends ParameterPropertyBase {
        /**
        * Creates a TypeScript variable element.
        */
        constructor();
    }
}
declare module TypeDocs {
    enum ElementType {
        None = 0,
        Module = 1,
        Class = 2,
        Interface = 3,
        Enum = 4,
        EnumValue = 5,
        Function = 6,
        Indexer = 7,
        Parameter = 8,
        Property = 9,
        Constructor = 10,
        TypeParameter = 11,
    }
}
/*******************************************************************************
Copyright (c) Alvaro Rahul Dias. All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*******************************************************************************/
declare module TypeDocs {
    /**
    * Provides the capability to generate object graph with documentation.
    */
    class Generator {
        private _underscoreIsPrivate;
        private _inputs;
        private _parseOptions;
        private _modules;
        private _modulesWithElements;
        /**
        * Creates a new Generator.
        *
        * @param inputs The TypeScript source to generate documentation for.
        * @param options Options used for controlling how documentation generation happens.
        */
        constructor(inputs: GeneratorInput[], options: GeneratorOptions);
        /**
        * The list of root modules in a tree structure.
        */
        public modules : Syntax.Module[];
        /**
        * The flattened list of modules that contain at least one non-module element.
        */
        public modulesWithElements : Syntax.Module[];
        private _tree;
        public process(): void;
        static toJson(generator: Generator): string;
        private _findAndProcessModules(element, parentModule, rootModuleNameStack?);
        private _getModulesWithElements();
        private createFunctionDoc(tsFunction, docsElement, alwaysGenerate?);
        private static createTypeParameterDocs(typeParameters);
        private createClassDoc(tsClass);
        private createInterfaceDoc(tsInterface);
        private static createHeritageClausesDocs(tsHeritageClauses);
        private static createIndexerDoc(tsIndexer);
        private createPropertyDoc(tsProperty, docsElement, alwaysGenerate?);
        private createEnumDoc(tsEnum);
        private static getJsDocComment(element);
        private static _setModifiers(modifiers, element);
    }
}
declare var exports: any;
declare module TypeDocs.JsDocParser {
    /**
    * Defines the attributes of a JsDoc comment.
    */
    class JsDocComment {
        /**
        * The description of the comment.
        */
        public description: string;
        /**
        * The parameters defined in the comment by the @param tag as a string map.
        */
        public parameters: StringMap<string>;
        /**
        * The return value defined in the comment by the @return or @returns tag.
        */
        public returns: string;
    }
}
declare module TypeDocs.JsDocParser {
    /**
    * Parses the specified input and creates the JsDoc comment.
    *
    * @param commentText The input string to be parsed.
    * @return The JsDoc comment.
    */
    function parse(commentText: string): JsDocComment;
}
interface Array<T> {
    /**
    * Returns the first element of an array that matches the predicate.
    *
    * @param predicate The Predicate function.
    * @return The first element that matches the predicate.
    */
    first(predicate?: (value: T) => boolean): T;
}
declare module TypeDocs.PolyFills {
}
