using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
    class NewMaze
    {
        static int wid, hgt;

        // Build the maze nodes.
        MazeNode[,] nodes = MakeNodes(wid, hgt);

        private static MazeNode[,] MakeNodes(int wid, int hgt)
        {
            MazeNode[,] maze = new MazeNode[wid, hgt];

            for(int w = 0; w < wid; w++)
            {
                for (int h = 0; h < hgt; h++)
                {
                    if(maze[w + 1, h] != null)
                    {
                        maze[w, h].Neighbors[0] = maze[w + 1, h];
                    }
                    if (maze[w - 1, h] != null)
                    {
                        maze[w, h].Neighbors[0] = maze[w - 1, h];
                    }
                    if (maze[w, h + 1] != null)
                    {
                        maze[w, h].Neighbors[0] = maze[w, h + 1];
                    }
                    if (maze[w, h - 1] != null)
                    {
                        maze[w, h].Neighbors[0] = maze[w, h - 1];
                    }
                }
            }

            return maze;
        }

        // Build a spanning tree with the indicated root node.
        private void FindSpanningTree(MazeNode root)
        {
            Random rand = new Random();

            // Set the root node's predecessor so we know it's in the tree.
            root.Predecessor = root;

            // Make a list of candidate links.
            List<MazeLink> links = new List<MazeLink>();

            // Add the root's links to the links list.
            foreach (MazeNode neighbor in root.Neighbors)
            {
                if (neighbor != null)
                    links.Add(new MazeLink(root, neighbor));
            }

            // Add the other nodes to the tree.
            while (links.Count > 0)
            {
                // Pick a random link.
                int link_num = rand.Next(0, links.Count);
                MazeLink link = links[link_num];
                links.RemoveAt(link_num);

                // Add this link to the tree.
                MazeNode to_node = link.ToNode;
                link.ToNode.Predecessor = link.FromNode;

                // Remove any links from the list that point
                // to nodes that are already in the tree.
                // (That will be the newly added node.)
                for (int i = links.Count - 1; i >= 0; i--)
                {
                    if (links[i].ToNode.Predecessor != null)
                        links.RemoveAt(i);
                }

                // Add to_node's links to the links list.
                foreach (MazeNode neighbor in to_node.Neighbors)
                {
                    if ((neighbor != null) && (neighbor.Predecessor == null))
                        links.Add(new MazeLink(to_node, neighbor));
                }
            }
        }
    }
}
