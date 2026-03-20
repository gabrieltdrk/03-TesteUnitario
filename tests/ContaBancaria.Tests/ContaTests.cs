using Xunit;
using ContaBancaria;

namespace ContaBancaria.Tests;

/// <summary>
/// Testes unitários para a classe Conta.
/// 
/// PARTE 1 — Testes de exemplo (Construtor) já estão prontos.
///           Observe o padrão AAA e o uso de [Fact] e [Theory].
///
/// PARTE 2 — Testes escritos com TDD para Depositar, Sacar,
///           Transferir e Encerrar.
/// </summary>
public class ContaTests
{
    // =======================================================
    //  PARTE 1 — EXEMPLO GUIADO: Testes do Construtor
    // =======================================================

    [Fact]
    public void Construtor_DadosValidos_CriaContaCorretamente()
    {
        // Arrange & Act
        var conta = new Conta("Maria", 100);

        // Assert
        Assert.Equal("Maria", conta.Titular);
        Assert.Equal(100, conta.Saldo);
        Assert.True(conta.Ativa);
    }

    [Fact]
    public void Construtor_SemSaldoInicial_CriaContaComSaldoZero()
    {
        // Arrange & Act
        var conta = new Conta("João");

        // Assert
        Assert.Equal("João", conta.Titular);
        Assert.Equal(0, conta.Saldo);
        Assert.True(conta.Ativa);
    }

    [Fact]
    public void Construtor_TitularNulo_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Conta(null!));
    }

    [Fact]
    public void Construtor_TitularVazio_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Conta(""));
    }

    [Fact]
    public void Construtor_SaldoNegativo_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Conta("Maria", -50));
    }

    [Theory]
    [InlineData("Ana", 0)]
    [InlineData("Carlos", 1000)]
    [InlineData("Beatriz", 0.01)]
    public void Construtor_VariosValoresValidos_CriaContaCorretamente(string titular, decimal saldo)
    {
        // Act
        var conta = new Conta(titular, saldo);

        // Assert
        Assert.Equal(titular, conta.Titular);
        Assert.Equal(saldo, conta.Saldo);
        Assert.True(conta.Ativa);
    }

    // =======================================================
    //  PARTE 2 — TESTES (TDD)
    //  Ciclo: 🔴 Escreva → falhe | 🟢 Implemente → passe | 🔵 Refatore
    // =======================================================

    // =======================================================
    //  Depositar
    // =======================================================

    [Fact]
    public void Depositar_ValorValido_AtualizaSaldo()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act
        conta.Depositar(50);

        // Assert
        Assert.Equal(150, conta.Saldo);
    }

    [Fact]
    public void Depositar_ValorZero_LancaArgumentException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => conta.Depositar(0));
    }

    [Fact]
    public void Depositar_ValorNegativo_LancaArgumentException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => conta.Depositar(-10));
    }

    [Fact]
    public void Depositar_ContaInativa_LancaInvalidOperationException()
    {
        // Arrange — encerra a conta antes (saldo precisa ser zero)
        var conta = new Conta("Maria", 0);
        conta.Encerrar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => conta.Depositar(50));
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(1)]
    [InlineData(9999.99)]
    public void Depositar_VariosValoresPositivos_AtualizaSaldo(decimal valor)
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act
        conta.Depositar(valor);

        // Assert
        Assert.Equal(100 + valor, conta.Saldo);
    }

    // =======================================================
    //  Sacar
    // =======================================================

    [Fact]
    public void Sacar_ValorValido_AtualizaSaldo()
    {
        // Arrange
        var conta = new Conta("Maria", 200);

        // Act
        conta.Sacar(80);

        // Assert
        Assert.Equal(120, conta.Saldo);
    }

    [Fact]
    public void Sacar_ValorMaiorQueSaldo_LancaInvalidOperationException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => conta.Sacar(150));
    }

    [Fact]
    public void Sacar_ValorZero_LancaArgumentException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => conta.Sacar(0));
    }

    [Fact]
    public void Sacar_ValorNegativo_LancaArgumentException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => conta.Sacar(-30));
    }

    [Fact]
    public void Sacar_ContaInativa_LancaInvalidOperationException()
    {
        // Arrange
        var conta = new Conta("Maria", 0);
        conta.Encerrar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => conta.Sacar(10));
    }

    [Fact]
    public void Sacar_ValorIgualAoSaldo_ZeraSaldo()
    {
        // Arrange — caso de borda: saque exato do saldo disponível
        var conta = new Conta("Maria", 100);

        // Act
        conta.Sacar(100);

        // Assert
        Assert.Equal(0, conta.Saldo);
    }

    // =======================================================
    //  Transferir
    // =======================================================

    [Fact]
    public void Transferir_ValorValido_AtualizaSaldoDeAmbasContas()
    {
        // Arrange
        var origem = new Conta("Maria", 200);
        var destino = new Conta("João", 100);

        // Act
        origem.Transferir(destino, 50);

        // Assert
        Assert.Equal(150, origem.Saldo);
        Assert.Equal(150, destino.Saldo);
    }

    [Fact]
    public void Transferir_SaldoInsuficiente_LancaInvalidOperationException()
    {
        // Arrange
        var origem = new Conta("Maria", 50);
        var destino = new Conta("João", 100);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => origem.Transferir(destino, 200));
    }

    [Fact]
    public void Transferir_ValorZero_LancaArgumentException()
    {
        // Arrange
        var origem = new Conta("Maria", 200);
        var destino = new Conta("João", 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => origem.Transferir(destino, 0));
    }

    [Fact]
    public void Transferir_ValorNegativo_LancaArgumentException()
    {
        // Arrange
        var origem = new Conta("Maria", 200);
        var destino = new Conta("João", 100);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => origem.Transferir(destino, -50));
    }

    [Fact]
    public void Transferir_ContaOrigemInativa_LancaInvalidOperationException()
    {
        // Arrange
        var origem = new Conta("Maria", 0);
        var destino = new Conta("João", 100);
        origem.Encerrar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => origem.Transferir(destino, 50));
    }

    [Fact]
    public void Transferir_ContaDestinoInativa_LancaInvalidOperationException()
    {
        // Arrange
        var origem = new Conta("Maria", 200);
        var destino = new Conta("João", 0);
        destino.Encerrar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => origem.Transferir(destino, 50));
    }

    // =======================================================
    //  Encerrar
    // =======================================================

    [Fact]
    public void Encerrar_ContaComSaldoZero_DesativaAConta()
    {
        // Arrange
        var conta = new Conta("Maria", 0);

        // Act
        conta.Encerrar();

        // Assert
        Assert.False(conta.Ativa);
    }

    [Fact]
    public void Encerrar_ContaComSaldo_LancaInvalidOperationException()
    {
        // Arrange
        var conta = new Conta("Maria", 100);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => conta.Encerrar());
    }

    [Fact]
    public void Encerrar_ContaJaInativa_LancaInvalidOperationException()
    {
        // Arrange
        var conta = new Conta("Maria", 0);
        conta.Encerrar(); // primeiro encerramento

        // Act & Assert — segundo encerramento deve falhar
        Assert.Throws<InvalidOperationException>(() => conta.Encerrar());
    }

    [Fact]
    public void Encerrar_ContaEncerrada_AtivaEhFalse()
    {
        // Arrange
        var conta = new Conta("Maria", 0);

        // Act
        conta.Encerrar();

        // Assert — garante que a propriedade Ativa reflete o encerramento
        Assert.False(conta.Ativa);
    }
}