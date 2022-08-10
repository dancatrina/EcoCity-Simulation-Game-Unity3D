using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerResurse
{
    public List<AProdusFerma> listMateriiPrime;
    public List<AProdusIndustrial> listProduse;

    public ContainerResurse()
    {
        listMateriiPrime = new List<AProdusFerma>();
        listMateriiPrime.Add(new ProdusFermaGrau(0));
        listMateriiPrime.Add(new ProdusFermaBoabeCafea(0));
        listMateriiPrime.Add(new ProdusFermaTutun(0));

        listProduse = new List<AProdusIndustrial>();
        listProduse.Add(new ProdusIndustrialCafea(0));
        listProduse.Add(new ProdusIndustrialPaine(0));
        listProduse.Add(new ProdusIndustrialTigara(0));

    }

    public void setCantitateMateriePrima(EMateriePrima tip, int cantitate)
    {
        for(int i = 0; i< listMateriiPrime.Count; i++)
            if(tip == listMateriiPrime[i].TipMateriaPrima)
            {
                listMateriiPrime[i].CantitateProdus = cantitate;
                break;
            }
    }

    public void setCantitateProdus(EProdusIndustrial tip, int cantitate)
    {
        for(int i= 0;i < listProduse.Count; i++)
        {
            if(tip == listProduse[i].getTipProdus())
            {
                listProduse[i].setCantitateProdus(cantitate);
            }
        }
    }

    public int getCantitateMateriePrima(EMateriePrima tip)
    {
        for (int i = 0; i < listMateriiPrime.Count; i++)
            if (tip == listMateriiPrime[i].TipMateriaPrima)
            {
                return listMateriiPrime[i].CantitateProdus;
            }
        return 0;
    }

    public int getCantitateProdus(EProdusIndustrial tip)
    {
        for (int i = 0; i < listProduse.Count; i++)
        {
            if (tip == listProduse[i].getTipProdus())
            {
                return listProduse[i].getCantitateProdus();
            }
        }

        return 0;
    }

    public int getTotalResurse()
    {
        int suma = 0;
        for(int i=0;i< listMateriiPrime.Count;++i)
        {
            suma += listMateriiPrime[i].CantitateProdus;
        }

        for(int i=0; i< listProduse.Count;i++)
        {
            suma += listProduse[i].getCantitateProdus();
     }
        return suma;
    }

    public override string ToString()
    {
        return "1: " + listMateriiPrime[0].CantitateProdus + " 2: " + listMateriiPrime[1].CantitateProdus +
            " 3: " + listMateriiPrime[2].CantitateProdus + " || 1:" + listProduse[0].getCantitateProdus() +
            " 2:" + listProduse[1].getCantitateProdus() + " 3:" + listProduse[2].getCantitateProdus();
    }

    public void eliminaResurse(int cantitate)
    {
        if (cantitate == 0) return;

        int deEliminat = cantitate;
        for (int i = 0; i < listMateriiPrime.Count; i++)
        {
            if (listMateriiPrime[i].CantitateProdus - deEliminat < 0)
            {
                deEliminat = deEliminat - listMateriiPrime[i].CantitateProdus;
                listMateriiPrime[i].CantitateProdus = 0;
            }
            else
            {
                deEliminat = listMateriiPrime[i].CantitateProdus - deEliminat;
                listMateriiPrime[i].CantitateProdus = deEliminat;
            }
        }

        for (int i = 0; i < listProduse.Count; i++)
        {
            if (listProduse[i].getCantitateProdus() - deEliminat < 0)
            {
                deEliminat = deEliminat - listProduse[i].getCantitateProdus();
                listProduse[i].setCantitateProdus(0);
            }
            else
            {
                deEliminat = listProduse[i].getCantitateProdus() - deEliminat;
                listProduse[i].setCantitateProdus(deEliminat);
            }
        }
    }
}
