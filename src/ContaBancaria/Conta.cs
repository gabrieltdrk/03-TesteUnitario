namespace ContaBancaria;

/// <summary>
/// Representa uma conta bancária simples.
/// </summary>
public class Conta
{
    // =======================================================
    //  Propriedades
    // =======================================================

    /// <summary>Nome do titular da conta.</summary>
    public string Titular { get; private set; }

    /// <summary>Saldo atual da conta.</summary>
    public decimal Saldo { get; private set; }

    /// <summary>Indica se a conta está ativa.</summary>
    public bool Ativa { get; private set; }

    // =======================================================
    //  Construtor
    // =======================================================

    /// <summary>
    /// Cria uma nova conta bancária.
    /// </summary>
    /// <param name="titular">Nome do titular (não pode ser nulo ou vazio).</param>
    /// <param name="saldoInicial">Saldo inicial (deve ser >= 0).</param>
    /// <exception cref="ArgumentException">
    ///   Lançada se o titular for nulo/vazio ou o saldo inicial for negativo.
    /// </exception>
    public Conta(string titular, decimal saldoInicial = 0)
    {
        if (string.IsNullOrWhiteSpace(titular))
            throw new ArgumentException("O titular não pode ser nulo ou vazio.", nameof(titular));

        if (saldoInicial < 0)
            throw new ArgumentException("O saldo inicial não pode ser negativo.", nameof(saldoInicial));

        Titular = titular;
        Saldo = saldoInicial;
        Ativa = true;
    }

    // =======================================================
    //  Métodos
    // =======================================================

    /// <summary>
    /// Deposita um valor na conta.
    /// </summary>
    /// <param name="valor">Valor a depositar (deve ser > 0).</param>
    /// <exception cref="ArgumentException">Lançada se o valor for <= 0.</exception>
    /// <exception cref="InvalidOperationException">Lançada se a conta estiver inativa.</exception>
    public void Depositar(decimal valor)
    {
        if (!Ativa)
            throw new InvalidOperationException("Não é possível depositar em uma conta inativa.");

        if (valor <= 0)
            throw new ArgumentException("O valor do depósito deve ser maior que zero.", nameof(valor));

        Saldo += valor;
    }

    /// <summary>
    /// Saca um valor da conta.
    /// </summary>
    /// <param name="valor">Valor a sacar (deve ser > 0 e <= saldo).</param>
    /// <exception cref="ArgumentException">Lançada se o valor for <= 0.</exception>
    /// <exception cref="InvalidOperationException">
    ///   Lançada se a conta estiver inativa ou o saldo for insuficiente.
    /// </exception>
    public void Sacar(decimal valor)
    {
        if (!Ativa)
            throw new InvalidOperationException("Não é possível sacar de uma conta inativa.");

        if (valor <= 0)
            throw new ArgumentException("O valor do saque deve ser maior que zero.", nameof(valor));

        if (valor > Saldo)
            throw new InvalidOperationException("Saldo insuficiente para realizar o saque.");

        Saldo -= valor;
    }

    /// <summary>
    /// Transfere um valor para outra conta.
    /// </summary>
    /// <param name="destino">Conta de destino.</param>
    /// <param name="valor">Valor a transferir (deve ser > 0 e <= saldo).</param>
    /// <exception cref="ArgumentException">Lançada se o valor for <= 0.</exception>
    /// <exception cref="InvalidOperationException">
    ///   Lançada se qualquer conta estiver inativa ou o saldo for insuficiente.
    /// </exception>
    public void Transferir(Conta destino, decimal valor)
    {
        if (!Ativa)
            throw new InvalidOperationException("Não é possível transferir de uma conta inativa.");

        if (!destino.Ativa)
            throw new InvalidOperationException("Não é possível transferir para uma conta inativa.");

        if (valor <= 0)
            throw new ArgumentException("O valor da transferência deve ser maior que zero.", nameof(valor));

        if (valor > Saldo)
            throw new InvalidOperationException("Saldo insuficiente para realizar a transferência.");

        Saldo -= valor;
        destino.Saldo += valor;
    }

    /// <summary>
    /// Encerra a conta bancária.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///   Lançada se a conta já estiver inativa ou se o saldo for diferente de zero.
    /// </exception>
    public void Encerrar()
    {
        if (!Ativa)
            throw new InvalidOperationException("A conta já está encerrada.");

        if (Saldo != 0)
            throw new InvalidOperationException("Não é possível encerrar uma conta com saldo.");

        Ativa = false;
    }
}
