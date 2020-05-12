grammar diamond;

program       : 
                BEGIN START_STATEMENT
                statement*
                procedureUsing*
                END_STATEMENT END SEMICOLON
                procedureDeclaration*
              ;

statement     : 
                declaration
              | printstatement 
              | assignstatement 
              | loopsstatement
              | ifstatement 
              ;

procedureUsing: 
               IDENTIFIER LEFTPAREN RIGHTPAREN SEMICOLON 
              ;

procedureDeclaration : 
                types IDENTIFIER LEFTPAREN RIGHTPAREN START_STATEMENT
                statement*
                END_STATEMENT SEMICOLON
              ;

declaration   : 
                types* IDENTIFIER SEMICOLON 
              | types* assignstatement
              ;

types         : 
                INT
              | DOUBLE
              | BOOLEAN
              ;

ifstatement   : 
                IF LEFTPAREN identifier EQUAL integer RIGHTPAREN START_STATEMENT
                statement*
                END_STATEMENT
              | IF LEFTPAREN identifier EQUAL double RIGHTPAREN START_STATEMENT
                statement*
                END_STATEMENT
              ;

loopsstatement: 
                FOR LEFTPAREN assignstatement comparestatement SEMICOLON RIGHTPAREN START_STATEMENT
                statement*
                END_STATEMENT
              | DO START_STATEMENT 
                statement*
                END_STATEMENT WHILE LEFTPAREN comparestatement RIGHTPAREN SEMICOLON
              | WHILE LEFTPAREN comparestatement RIGHTPAREN START_STATEMENT
                statement*
                END_STATEMENT 
              ;  

printstatement: 
                PRINT LEFTPAREN QUOTI term QUOTI RIGHTPAREN SEMICOLON 
              | PRINT LEFTPAREN QUOTI QUOTI RIGHTPAREN SEMICOLON 
              ;

readstatement: 
                READ LEFTPAREN QUOTI term QUOTI RIGHTPAREN SEMICOLON 
              ;

assignstatement: 
                IDENTIFIER ASSIGN expression SEMICOLON 
              ;

comparestatement:
                identifier LESS integer
              | identifier GREATER integer
              | identifier LESSEQUAL integer
              | identifier GREATEREQUAL integer
              | identifier EQUAL integer
              | identifier NOTEQUAL integer
              ; 

expression    : 
                term
              | term PLUS term
              | term MINUS term
              | term DIVIDE term
              | term MULT term
              ;

term: 
                identifier
              | integer
              | double
              | bool
              ;

identifier:   IDENTIFIER;

integer:      INTEGER;
double:     DOUBLE;
bool:   BOOLEAN;

//Program
BEGIN: 'program';
END: 'endprogram';
RETURN: 'return';

PRINT: 'writeline';
READ: "readline";
QUOTI: '"';
START_STATEMENT: '{';
END_STATEMENT: '}';

// Type of vars
INT: 'int';
DOUBLE : 'double';
BOOLEAN : 'bool';

// Operators
PLUS: '+';
MINUS: '-';
DIVIDE: '/';
MULT: '*';
ASSIGN: '=';

// Compare operators
EQUAL: '==';
NOTEQUAL: '!=';
LESS: '<';
GREATER: '>';
LESSEQUAL: '=<';
GREATEREQUAL: '>=';

// Condition and loops
IF: 'if';
FOR: 'for';
DO: 'do';
WHILE: 'while';

// Semicolon and parentheses
SEMICOLON: ';';
RIGHTPAREN: ')';
LEFTPAREN: '(';

// Boolean
BOOL : 'true' | 'false';

// Numbers
DOUBLE : INT+ PT INT+
    | PT INT+
    | INT+
    ;
INT: [0-9][0-9]*;

// Point
PT : '.';

// Identifier
IDENTIFIER : FirstIdSymbol NameChar*;

fragment
NameChar
    :   FirstIdSymbol
    |   '0'..'9'
    |   '_'
    |   '\u00B7'
    |   '\u0300'..'\u036F'
    |   '\u203F'..'\u2040'
    ;

fragment
FirstIdSymbol
    :   'A'..'Z'
    |   'a'..'z'
    |   '\u00C0'..'\u00D6'
    |   '\u00D8'..'\u00F6'
    |   '\u00F8'..'\u02FF'
    |   '\u0370'..'\u037D'
    |   '\u037F'..'\u1FFF'
    |   '\u200C'..'\u200D'
    |   '\u2070'..'\u218F'
    |   '\u2C00'..'\u2FEF'
    |   '\u3001'..'\uD7FF'
    |   '\uF900'..'\uFDCF'
    |   '\uFDF0'..'\uFFFD'
    ;

//Empty space, New line
WhiteSpace: [ \t\r\n]+ -> skip ;