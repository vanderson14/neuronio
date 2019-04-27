
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<decimal[]> matriz = new List<decimal[]>();
        matriz.Add(new decimal[] { 1, 1 });
        matriz.Add(new decimal[] { 0, 1 });
        matriz.Add(new decimal[] { 1, 0 });
        matriz.Add(new decimal[] { 0, 0 });

        List<decimal> valoresEsperados = new List<decimal>();
        valoresEsperados.Add(1);
        valoresEsperados.Add(1);
        valoresEsperados.Add(1);
        valoresEsperados.Add(-1);

        var resultado = perception(100, 1, 0.1M, matriz, valoresEsperados);

        Response.Write(resultado);
    }

    public object perception(int maxInteracoes, decimal taxaErro, decimal alfa, List<decimal[]> matriz, List<decimal> valoresEsperados)
    {
        decimal[] w = { 0.6M, 0.5M };
        decimal b = -0.3M;
        int t = 1;
        decimal[] erro = { 0, 0, 0, 0 };

        while (t < maxInteracoes && taxaErro != 0)
        {
            for (int x = 0; x < matriz.Count; x++)
            {
                decimal y = 0;

                y += funcaoAtivaca(matriz[x], w, b) > 0 ? 1 : -1;

                erro[x] = valoresEsperados[x] - y;

                for (int z = 0; z < matriz[x].Count(); z++)
                {
                    w[z] = w[z] + (alfa * erro[x] * matriz[x][z]);
                }

                b = b + (alfa * erro[x]);
            }

            taxaErro = erro.Where(x => x != 0).Count();
            t++;
        }

        return new { pesos = w, b = b };
    }

    public decimal funcaoAtivaca(decimal[] valores, decimal[] pesos, decimal b)
    {
        var resultado = 0M;

        for (int x = 0; x < valores.Count(); x++)
        {
            resultado += valores[x] * pesos[x];
        }

        resultado += b;

        return resultado;
    }

}
