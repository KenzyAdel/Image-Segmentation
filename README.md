# 🖼️ Image Segmentation Using Graph-Based Approach (Algorithms Project 2025)

## 📖 Overview
This project implements a **graph-based image segmentation algorithm** that partitions a digital image into meaningful regions based on pixel similarity and boundary strength.  
The segmentation follows two main principles:

1. **Internal Coherence** – Pixels within a region are visually similar.  
2. **Boundary Significance** – Neighboring regions differ enough to justify a boundary.

Image segmentation is a fundamental step in many computer vision applications such as:
- Preprocessing for deep-learning models  
- Automatic figure–ground separation  
- Interactive photo-editing tools  

---

## ⚙️ Methodology

### 1. Graph Representation
Each **pixel** is represented as a **vertex**, and edges connect it to its **8 neighboring pixels** (8-connected grid).

- **Vertices (V):** Represent individual pixels  
- **Edges (E):** Connect adjacent pixels  
- **Edge Weight:** Absolute intensity difference between connected pixels  

Before graph construction, a **Gaussian blur** is applied to reduce noise while preserving visible content.

#### 🟥 Handling Color Images
Color images are processed as three independent grayscale layers:
1. Run segmentation separately on **Red**, **Green**, and **Blue** channels.  
2. Intersect results — two pixels belong to the same final region only if connected in all three channels.

---

### 2. Region-Finding Strategy
The algorithm merges or separates regions by comparing:
- **Between-region dissimilarity**, and  
- **Within-region variability.**

A boundary is **kept** when the difference between regions is significant relative to internal variation. Otherwise, regions are **merged**.

#### Key Concepts
- **Internal Difference:** Largest edge weight necessary to keep a component connected.  
- **Difference Between Components:** Minimum edge weight connecting two regions.  
- **Predicate Function:** Determines boundary strength using an adaptive threshold proportional to region size.  

This ensures boundaries are perceptually meaningful even in complex images.

---

## 🧮 Implementation Details

### ✅ Completed Requirements
1. **Graph Construction & Weight Computation**  
   - Converts image to a weighted, undirected graph.  
   - Complexity: O(N²), where *N* is one image dimension.  

2. **Segmentation Algorithm**  
   - Adaptive region-merging based on intensity comparison.  
   - Complexity: O(M log M), where *M* is the number of pixels.  

3. **Visualization**  
   - Distinct colors assigned per region and overlaid on the original image for clarity.  

---

## 🧩 Input & Output

### **Input**
- Image (2D pixel array)  
- Parameter **K** – threshold control constant  

### **Output**
1. **Text file** containing:  
   - Total number of segments  
   - Each segment’s size (sorted descending)  
2. **Visualization image** showing color-coded segmentation overlay.

---

## 🧪 Testing

### **Sample Test**
- Used to verify segmentation correctness on sample images.

### **Complete Test**
Evaluates performance and scalability across three levels:
- **Small:** ~100 KB  
- **Medium:** ~10 MB  
- **Large:** ~100 MB  

---

## 📊 Performance Notes
All required tasks meet the performance constraints:
- **Graph Construction:** ≤ O(N²)  
- **Segmentation Algorithm:** ≤ O(M log M)  
- **Visualization:** Efficient rendering of large images  

---

## 🧰 Tools & Environment
- **Language:** C# (.NET Framework / Windows Forms)  
- **Starter Code:** Provided `ImageOperations` class includes:
  - Image loading and display  
  - Gaussian filtering  
  - Width/height accessors  
  - RGB pixel structure (`RGBPixel`)  

---

## 🖼️ Results Preview
The algorithm was validated on several test images:
- **Street scene** (320×240, color)  
- **Baseball scene** (432×294, grayscale)  
- **Indoor scene** (320×240, color)  

Each produced visually accurate region segmentation consistent with human perception.

---

## 🏆 Bonus Implementation

### ✅ Bonus B2 — Interactive “Click-to-Merge” UI
A post-segmentation interactive feature allowing users to **manually merge regions**:
- Click to select multiple colored regions  
- Press a key or button to merge them  
- The merged region appears in its original colors  

This improves segmentation quality where automatic processing may over-segment objects.

### ❌ Bonus B1 — Nearest-Neighbor Graph Segmentation
Not implemented.

---

## 📂 Deliverables
- **Implementation:**  
  - Graph construction  
  - Segmentation algorithm  
  - Visualization  
  - Interactive “Click-to-Merge” UI (Bonus B2)  
- **Documentation:**  
  - Code analysis  
  - Complexity and algorithmic discussion  

---

## 📊 Summary

| Component | Status | Description |
|------------|---------|-------------|
| Graph Construction | ✅ | 8-connected pixel graph |
| Segmentation Algorithm | ✅ | Adaptive region-merging based on intensity differences |
| Visualization | ✅ | Color-coded region overlay |
| Documentation | ✅ | Complete and detailed |
| Bonus B1 (Nearest-Neighbor Graph) | ❌ | Not implemented |
| Bonus B2 (Click-to-Merge UI) | ✅ | Fully implemented interactive merge feature |

