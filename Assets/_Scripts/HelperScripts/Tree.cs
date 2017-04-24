using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class TreeNode<T>//CS260 FTW
{
    private Dictionary<string, TreeNode<T>> allNodes;
    public TreeNode<T> parent;
    private Dictionary<T, TreeNode<T>> children;
    public T Data;
    private string path;
    private string dataString;
    public int depth;
    public int number;
    public bool end;

    public Dictionary<T, TreeNode<T>> Children(Predicate<T> condition = null)
    {

        if (children == null)
        {
            children = new Dictionary<T, TreeNode<T>>();
            Debug.Log(Data + "'s children was null, so it was created.");
        }

        //children=children.where

        return children;
    }


    public string Path()
    {
        if (parent == null)
        {
            return DataString;
        }
        else {

            return parent.Path() + "/" + DataString;
        }
    }

    public Dictionary<string, TreeNode<T>> AllNodes
    {
        get
        {
            if (allNodes == null) { allNodes = new Dictionary<string, TreeNode<T>>(); }
            if (parent == null)
            {
                if (allNodes == null)
                {
                    allNodes = new Dictionary<string, TreeNode<T>>();
                }
                return allNodes;
            }
            else { return parent.allNodes; }
        }

        set
        {
            if (parent == null)
            {
                allNodes = value;
            }
            else { parent.AllNodes = value; }
        }
    }

    public string DataString
    {
        get
        {
            return Data.ToString();
        }

        set
        {
            dataString = value;
        }
    }

    public void InsertChild(TreeNode<T> newChild)
    {
        newChild.parent = this;
        children.Add(newChild.Data, newChild);
        newChild.depth = depth + 1;
        newChild.number = children.Count - 1;
        if (!AllNodes.ContainsKey(Path())) { AllNodes.Add(Path(), newChild); }
    }
    public void RemoveChild(TreeNode<T> child)
    {
        AllNodes.Remove(child.path);
        children.Remove(child.Data);
    }
    public TreeNode(T d)
    {
        Data = d;
        children = new Dictionary<T, TreeNode<T>>();
        number = 0;
    }
    public TreeNode<T> Get(T d)
    {
        return children[d];
    }
    public TreeNode<T> GetChildByIndex(int n)
    {
        foreach (TreeNode<T> t in children.Values)
        {
            if (t.number == n) { return t; }
        }
        return null;
    }
    public static void DeleteChildren(TreeNode<T> node, bool preserve)
    {//TODO: Make it recursive
        if (node.children.Count > 0)
        {
            foreach (TreeNode<T> n in node.children.Values)
            {
                node.children = null;
            }
        }
        if (!preserve) { node = null; }
    }
    public bool HasChildren()
    {
        if (children != null)
        {
            if (children.Count > 0)
            {
                return true;
            }
        }
        return false;
    }

}
