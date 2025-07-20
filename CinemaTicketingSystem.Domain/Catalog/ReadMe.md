
None = 0        → 0000000 (hiçbir bit set değil)
Standard = 1    → 0000001 (1. bit)
IMAX = 2        → 0000010 (2. bit)  
ThreeD = 4      → 0000100 (3. bit)
FourDX = 8      → 0001000 (4. bit)
ScreenX = 16    → 0010000 (5. bit)
DolbyAtmos = 32 → 0100000 (6. bit)
DolbyCinema = 64→ 1000000 (7. bit)


Birden Fazla Değer Kombinasyonu
// Bir salon hem IMAX hem de DolbyAtmos destekleyebilir
var technologies = HallTechnology.IMAX | HallTechnology.DolbyAtmos;
// Binary: 0000010 | 0100000 = 0100010 (değer: 34)



Bitwise Operasyonlar
// Ekleme
technologies |= HallTechnology.ThreeD;

// Çıkarma  
technologies &= ~HallTechnology.IMAX;

// Kontrol etme
bool hasIMAX = technologies.HasFlag(HallTechnology.IMAX);


Veritabanında Efficient Storage

// Tek bir integer field'da birden fazla teknoloji saklanabilir
public int SupportedTechnologies { get; set; } // 34 = IMAX + DolbyAtmos