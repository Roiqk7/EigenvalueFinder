# Developer documentation

**TODO:**

## Algorithm summary

### QR Algorithm

**Input:** Matrix $A \in \mathbb{R}^{n \times n}$.

1.  Initialize $A_0 := A$, $i := 0$.
2.  **while** termination condition is not met **do**
3.  Construct the QR decomposition of matrix $A_i$, i.e., $A_i = QR$.
4.  Update $A_{i+1} := RQ$.
5.  Increment $i := i + 1$.
6.  **end while**

**Output:** Matrix $A_i$.

### QR Decomposition Algorithm

**Input:** Matrix $A \in \mathbb{R}^{m \times n}$.

1.  Initialize $Q := I_m$, $R := A$.
2.  **for** $j := 1$ to $\min(m, n)$ **do**
3.  Set $x := R(j:m, j)$. (This typically means the subvector of $R$ from row $j$ to $m$ in column $j$).
4.  **if** $x \neq \|x\|_2 e_1$ **then**
5.  Update $x := x - \|x\|_2 e_1$.
6.  Construct Householder matrix $H(x) := I_{m-j+1} - \frac{2}{x^T x} xx^T$.
7.  Construct $H := \begin{pmatrix} I_{j-1} & 0 \\ 0 & H(x) \end{pmatrix}$.
8.  Update $R := HR$, $Q := QH$.
9.  **end if**
10. **end for**
