<?php

/*Dennys Corporation, 2018. All right reserved. Usage for education only.*/
/*Dennys Corporation, 2018. Todos os direitos reservados. Somente para uso educacional.*/

/*===== FUNÇÕES USADAS =====*/
/*

trim                   = Limpar os caracteres de uma string(variável de texto).
isset                  = Verificar se uma variável já existe.
is_null                = Verificar se uma variável é vazia.
is_numeric             = Verificar se o tipo da variável é um número(int, double, long...).
is_string              = Verificar se o tipo da variável é uma string.
intval                 = Pegar o valor em int através de uma variável.
strlen                 = Pegar o número de caracteres de uma string.

outros:
Switch(Condição) = if;
return = retorno de uma função;
_GET | _POST = Tipo de solicitação recebida pelo servidor.


*/
/*===== FUNÇÕES USADAS =====*/

$numeros        = array(1024);
$numberofarray  = 0;

$operadores     = array(1024);
$operadorarray  = 0;

/*echo "M&#233todo de entrada: \"" . $_SERVER["REQUEST_METHOD"] . "\", ";
if($_SERVER["REQUEST_METHOD"] != "GET" && $_SERVER["REQUEST_METHOD"] != "POST")
{
    echo "inv&#225lido.";
    return;
}
else
    echo "v&#225lido. </br>"; //código em ascii html dos caracteres -> tabela: https://www.ime.usp.br/~glauber/html/acentos.htm*/

$TIPOOP = 'null';
if(isset($_GET['TIPO']))
    $TIPOOP = $_GET['TIPO'];
if(isset($_POST['TIPO']))
    $TIPOOP = $_POST['TIPO'];

if($TIPOOP == 'null')
{
    echo 'Tipo de opera&#231;&#227;o n&#227;o especificada.';
    return;
}

if($TIPOOP == 1)
{
for($i = 1; $i < 1024; $i+=1)
{
    if(isset($_GET["N".$i]) == true || isset($_POST["N".$i]) == true)
    {
        if($_SERVER['REQUEST_METHOD'] == "GET")
            $numeros[$numberofarray] = $_GET["N".$i];
        else if($_SERVER['REQUEST_METHOD'] == "POST")
            $numeros[$numberofarray] = $_POST["N".$i];
        
        if(!is_numeric($numeros[$numberofarray]))
        {
            OperacaoInvalida();
            return; //return (valor da função) não deixa a função seguir em frente e retorna um valor, no caso não retornou nada.
        }
        
        $numberofarray++;
    }
    if(isset($_GET["OP".$i]) == true || isset($_POST["OP".$i]) == true)
    {
        if($_SERVER['REQUEST_METHOD'] == "GET")
            $operadores[$operadorarray] = $_GET["OP".$i];
        else if($_SERVER['REQUEST_METHOD'] == "POST")
            $operadores[$operadorarray] = $_POST["OP".$i];
        
        if(VerificarOperador($operadores[$operadorarray]) == false)
        {
            OperacaoInvalida();
            return;
        }
        $operadorarray++;
    }
}

if($numberofarray != ($operadorarray+1))
{
    OperacaoInvalida();
    return;
}

/*echo "N&#250meros: " . $numberofarray . "</br>";
echo "Operadores: " . $operadorarray . "</br></br>";*/

$resultado = "null";

$number_1 = "";
$number_2 = "";
$operat__ = "";
    
$numberprint;

for($i = 0; $i < 1024; $i++)
{      
    if($i < $numberofarray)
    {
        $numberprint = $numeros[$i];
        
        if($resultado == "null") 
            $resultado = $numeros[$i];
        
        if(strpos($numeros[$i], '.') > 0)
            $numberprint[strpos($numeros[$i], '.')] = ',';
        
        echo $numberprint;
    }
        
    if($i < $operadorarray)
    {   
        echo $operat__ = $operadores[$i];
        
        if(isset($resultado))
            $resultadoantigo = $resultado;
        
        if($operat__ == '-' && $numeros[intval($i+1)] == $numeros[intval($i)])
            $resultado = '0';
        else
            $resultado = RealizarOperacao($resultado, $operat__, $numeros[intval($i+1)]);
        
    }
    else break; //break -> sair do loop
}

echo "=" . $resultado;

}

if($TIPOOP == 2)
{
    $base = "";
    $expoente = "";
    
    if(isset($_GET['Number1'], $_GET['Number2']))
    {
        $base       = $_GET['Number1'];
        $expoente   = $_GET['Number2'];
    }
    else if(isset($_POST['Number1'], $_POST['Number2']))
    {
        $base       = $_POST['Number1'];
        $expoente   = $_POST['Number2'];
    }
    else
    {
        OperacaoInvalida();
        return;
    }
    
    if(!is_numeric($base) || !is_numeric($expoente))
    {
        OperacaoInvalida();
        return;
    }
   //echo "Base: " . $base . "</br> Expoente: " . $expoente . "</br>Resultado: " . 
    echo OperarRadiciacao($base, $expoente);
}

if($TIPOOP == 3)
{
    $peso = "";
    $altura = "";
    
    if(isset($_GET['Number1'], $_GET['Number2']))
    {
        $peso       = $_GET['Number1'];
        $altura   = $_GET['Number2'];
    }
    else if(isset($_POST['Number1'], $_POST['Number2']))
    {
        $peso       = $_POST['Number1'];
        $altura   = $_POST['Number2'];
    }
    else
    {
        OperacaoInvalida();
        return;
    }
    
    if(!is_numeric($peso) || !is_numeric($altura))
    {
        OperacaoInvalida();
        return;
    }
    
   //echo "Base: " . $base . "</br> Expoente: " . $expoente . "</br>Resultado: " . 
    echo CalcularIMC($peso, $altura);
}


function OperarRadiciacao($_base, $_expoente)
{
    if(!is_numeric($_base) || !is_numeric($_expoente))
    {
        OperacaoInvalida();
        return;
    }
    $resultado = $_base;
    for($i = 0; $i < ($_expoente-1); $i+=1)
        $resultado*=$_base;
    
    return $resultado;
}
                        
function OperacaoInvalida()
{
    echo "Opera&#231;&#227;o inv&#225;lida.";
    //Ex: &#225; = 'á' (caracteres ascii html)
}

function CalcularIMC($_peso, $_altura)
{
    $resultado = $_peso / OperarRadiciacao($_altura, 2);
    
    return $resultado;
}

function VerificarOperador($varcheck)
    {
         switch($varcheck)
             {
             case '+':
             case '-':
             case '/':
             case 'x':
             case '*':
             return true;
             break;         /*caso for algum desses operadores, essa função irá retornar true, como válido*/
                            /*caso contrário irá retornar false, como inválido.*/
             default:
             return false;
             }
    return false;
    }
    
    function RealizarOperacao($__number1,$__operation, $__number2)
    {
             switch($__operation)
             {
             case '+':
                 return $__number1 + $__number2;
                 break;
                 
             case '-':
                     if($__number1 == $__number2)
                         return 0;
                 return $__number1 - $__number2;
                 break;
                 
             case 'x':
             case '*':
                 return $__number1 * $__number2;
                 break;
                 
             case '/':
                  if(intval($__number2) == 0)
                      return "N&#227;o &#233; poss&#237;vel realizar a divis&#227;o por zero.";
             else
                 return $__number1 / $__number2;
                 break;

             default:
                 return "Operador inv&#225;lido.";
             }
    }
?>