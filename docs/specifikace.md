# Algoritmus pro nalezení vlastních čísel)

## 1. Idea a základní popis problému

Program pro nalezení vlastních čísel a vektorů pro danou matici.

## 2. Formalizace problému

Problematika vlastních čísel je známá a hluboce prozkoumaná oblast lineární algebry. Jejich výpočet je ovšem netriviální. Použiji iteratativní `QR algoritmus`, který daná čísla nalezne. Samozřejmě také bude zapotřebí algoritmus pro nalezení QR rozkladu dané matice.

## 3. Základní popis algoritmu

Vstup:  matice \( A \in \mathbb{R}^{n \times n} \).

1:  $A_0 := A, \, i := 0$

2:  while not `splněna ukončovací podmínka` do

3:      sestroj QR rozklad matice $A_i = QR$

4:      $A_{i+1} := RQ$

5:      $i := i + 1$

6:  end while

Výstup:  matice $A_N$.

Kde ukončovací podmínka kontroluje, zda jsme dosáhli diagonální/horní trojúhelníkové matice nebo maxima iterací.

Výsledná matice $A_N$ po dostatku iterací konverguje k horní trojúhelníkové matici popřípadě diagonální. Na diagonále pak budou bloky o velikosti 1 a 2. Bloky o velikosti 1 jsou reálná vlastní čísla a bloky o velikosti 2 snadno dopočítatelná komplexně sdružená vlastní čísla.

## 4. Vstup a výstup

Vstup je matice typu `Matrix<double>` z knihovny `MathNet`. Výstup je seznam vlastích čísel a příslušných vektorů typu `List<Tuple<double, Vector<double>>>`.

## 5. Formát interakce

Jelikož můj projekt bude tvořit spíše knihovnu, interakce je flexibilní. Osobně volím jednoduché webové prostředí pro interakci s programem.
