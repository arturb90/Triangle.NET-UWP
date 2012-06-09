﻿// -----------------------------------------------------------------------
// <copyright file="Triangle.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet.Geometry;

    /// <summary>
    /// The triangle data structure.
    /// </summary>
    /// <remarks>
    /// Each triangle contains three pointers to adjoining triangles, plus three 
    /// pointers to vertices, plus three pointers to subsegments (declared below;
    /// these pointers are usually 'dummysub'). It may or may not also contain 
    /// user-defined attributes and/or a floating-point "area constraint".
    /// </remarks>
    public class Triangle : ITriangle
    {
        // Hash for dictionary. Will be set by mesh instance.
        internal int hash;

        // The ID is only used for mesh output.
        internal int id;

        internal Otri[] neighbors;
        internal Vertex[] vertices;
        internal Osub[] subsegs;
        internal double[] attributes;
        internal double area;
        internal bool infected;

        public Triangle(int numAttributes)
        {
            // Initialize the three adjoining triangles to be "outer space".
            neighbors = new Otri[3];
            neighbors[0].triangle = Mesh.dummytri;
            neighbors[1].triangle = Mesh.dummytri;
            neighbors[2].triangle = Mesh.dummytri;

            // Three NULL vertices.
            vertices = new Vertex[3];

            if (Behavior.UseSegments)
            {
                // Initialize the three adjoining subsegments to be the
                // omnipresent subsegment.
                subsegs = new Osub[3];
                subsegs[0].seg = Mesh.dummysub;
                subsegs[1].seg = Mesh.dummysub;
                subsegs[2].seg = Mesh.dummysub;
            }

            if (numAttributes > 0)
            {
                attributes = new double[numAttributes];
            }

            if (Behavior.VarArea)
            {
                area = -1.0;
            }
        }


        #region Public properties

        /// <summary>
        /// Gets the triangle id.
        /// </summary>
        public int ID
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the first corners vertex id.
        /// </summary>
        public int P0
        {
            get { return this.vertices[0] == null ? -1 : this.vertices[0].id; }
        }

        /// <summary>
        /// Gets the seconds corners vertex id.
        /// </summary>
        public int P1
        {
            get { return this.vertices[1] == null ? -1 : this.vertices[1].id; }
        }

        /// <summary>
        /// Gets the specified corners vertex id.
        /// </summary>
        public int this[int index]
        {
            get { return this.vertices[index] == null ? -1 : this.vertices[index].id; }
        }

        /// <summary>
        /// Gets the third corners vertex id.
        /// </summary>
        public int P2
        {
            get { return this.vertices[2] == null ? -1 : this.vertices[2].id; }
        }

        public bool SupportsNeighbors
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the first neighbors id.
        /// </summary>
        public int N0
        {
            get { return this.neighbors[0].triangle.id; }
        }

        /// <summary>
        /// Gets the second neighbors id.
        /// </summary>
        public int N1
        {
            get { return this.neighbors[1].triangle.id; }
        }

        /// <summary>
        /// Gets the third neighbors id.
        /// </summary>
        public int N2
        {
            get { return this.neighbors[2].triangle.id; }
        }

        /// <summary>
        /// Gets the triangle area constraint.
        /// </summary>
        public double Area
        {
            get { return this.area; }
        }

        /// <summary>
        /// Gets the triangle attributes.
        /// </summary>
        public double[] Attributes
        {
            get { return this.attributes; }
        }

        #endregion

        public override int GetHashCode()
        {
            return this.hash;
        }

        public override string ToString()
        {
            return String.Format("TID {0}", hash);
        }
    }
}